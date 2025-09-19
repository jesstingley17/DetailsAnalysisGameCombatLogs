using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class PersonalChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<PersonalChatMessageConsumer> logger,
    IServiceScopeFactory serviceScopeFactory, IMapper mapper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly ILogger<PersonalChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IMapper _mapper = mapper;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            using var scope = _serviceScopeFactory.CreateScope();

            await ExecuteAsync(scope, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Personal Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PersonalChatMessage, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var personalChatService = scope.ServiceProvider.GetService<IPersonalChatService>();
            ArgumentNullException.ThrowIfNull(personalChatService, nameof(personalChatService));

            var personalChatMessageHub = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(personalChatMessageHub, nameof(personalChatMessageHub));

            var personalChatUnreadMessageHub = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(personalChatUnreadMessageHub, nameof(personalChatUnreadMessageHub));

            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var personalChatMessageService = scope.ServiceProvider.GetService<IPersonalChatMessageService>();
            ArgumentNullException.ThrowIfNull(personalChatMessageService, nameof(personalChatMessageService));

            var personalChat = await personalChatService.GetByIdAsync(chatAction.ChatMessage.PersonalChatId);
            ArgumentNullException.ThrowIfNull(personalChat, nameof(personalChat));

            await personalChatMessageHub.ConnectToHubAsync($"{_hubs.Server}{_hubs.PersonalChatMessagesAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await personalChatMessageHub.JoinRoomAsync(chatAction.ChatMessage.PersonalChatId);

            await personalChatUnreadMessageHub.ConnectToHubAsync($"{_hubs.Server}{_hubs.PersonalChatUnreadMessageAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await personalChatUnreadMessageHub.JoinRoomAsync(chatAction.ChatMessage.PersonalChatId);

            switch (chatAction.State)
            {
                case ChatMessageActionState.Created:
                    var createdChatMessage = await CreateChatMessageAsync(personalChatMessageService, chatAction.ChatMessage);

                    await personalChatMessageHub.RequestMessageAsync(chatAction.ChatMessage.PersonalChatId, createdChatMessage);

                    if (chatAction.ChatMessage.Type == Chat.Domain.Enums.MessageType.Default)
                    {
                        await IncreaseUnreadMessageCountAsync(personalChat, chatAction.ChatMessage.AppUserId, personalChatService);
                        await personalChatUnreadMessageHub.RequestUnreadMessagesAsync(chatAction.ChatMessage.PersonalChatId, chatAction.ChatMessage.AppUserId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
                    }

                    break;
                case ChatMessageActionState.Read:
                    if (chatAction.ChatMessage.Type == Chat.Domain.Enums.MessageType.Default)
                    {
                        await ReadMessageAsync(personalChatMessageService, chatAction.ChatMessage);

                        await DecreaseUnreadMessageCountAsync(personalChat, chatAction.ChatMessage.AppUserId, personalChatService);

                        await personalChatUnreadMessageHub.RequestUnreadMessagesAsync(chatAction.ChatMessage.PersonalChatId, chatAction.ChatMessage.AppUserId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
                    }

                    break;
                case ChatMessageActionState.Edited:
                    break;
                default:
                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PersonalChatMessage, ex.ParamName);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat {Id} not found.", KafkaTopics.PersonalChatMessage, ex.EntityId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat not found or modified.", KafkaTopics.PersonalChatMessage);
        }
    }

    private async Task<PersonalChatMessageDto> CreateChatMessageAsync(IPersonalChatMessageService chatMessageService, PersonalChatMessageModel chatMessage)
    {
        var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
        var createdChatMessage = await chatMessageService.CreateAsync(map);

        return createdChatMessage;
    }

    private static async Task IncreaseUnreadMessageCountAsync(PersonalChatDto personalChat, string messageInitiatorId, IPersonalChatService personalChatService)
    {
        if (messageInitiatorId == personalChat.CompanionId)
        {
            personalChat.InitiatorUnreadMessages++;
        }
        else
        {
            personalChat.CompanionUnreadMessages++;
        }

        await personalChatService.UpdateChatAsync(personalChat.Id, personalChat.InitiatorUnreadMessages, personalChat.CompanionUnreadMessages);
    }

    private static async Task ReadMessageAsync(IPersonalChatMessageService chatMessageService, PersonalChatMessageModel chatMessage)
    {
        await chatMessageService.UpdateChatMessageAsync(chatMessage.Id, null, Chat.Domain.Enums.MessageStatus.Read, null);
    }

    private static async Task DecreaseUnreadMessageCountAsync(PersonalChatDto personalChat, string messageInitiatorId, IPersonalChatService personalChatService)
    {
        if (messageInitiatorId == personalChat.CompanionId)
        {
            personalChat.InitiatorUnreadMessages--;
        }
        else
        {
            personalChat.CompanionUnreadMessages--;
        }

        await personalChatService.UpdateChatAsync(personalChat.Id, personalChat.InitiatorUnreadMessages, personalChat.CompanionUnreadMessages);
    }
}
