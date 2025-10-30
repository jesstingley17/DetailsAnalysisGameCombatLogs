using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Interfaces;
using Chat.Application.Kafka.Actions;
using Chat.Application.Security;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Interfaces;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public class PersonalChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<PersonalChatMessageConsumer> logger,
    IServiceScopeFactory serviceScopeFactory, IChatHubHelper personalChatMessageHelper, IChatHubHelper personalChatUnreadMessageHelper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private readonly ILogger<PersonalChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IChatHubHelper _personalChatMessageHelper = personalChatMessageHelper;
    private readonly IChatHubHelper _personalChatUnreadMessageHelper = personalChatUnreadMessageHelper;

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

            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var personalChatMessageService = scope.ServiceProvider.GetService<IPersonalChatMessageService>();
            ArgumentNullException.ThrowIfNull(personalChatMessageService, nameof(personalChatMessageService));

            var personalChat = await personalChatService.GetByIdAsync(chatAction.ChatMessage.PersonalChatId);
            ArgumentNullException.ThrowIfNull(personalChat, nameof(personalChat));

            var accessToken = AesEncryption.Decrypt(chatAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));

            await _personalChatMessageHelper.ConnectToHubAsync(_hubs.PersonalChatMessagesAddress, accessToken);
            await _personalChatMessageHelper.JoinRoomAsync(chatAction.ChatMessage.PersonalChatId);

            await _personalChatUnreadMessageHelper.ConnectToHubAsync(_hubs.PersonalChatUnreadMessageAddress, accessToken);
            await _personalChatUnreadMessageHelper.JoinRoomAsync(chatAction.ChatMessage.PersonalChatId);

            switch (chatAction.State)
            {
                case ChatMessageActionState.Created:
                    var createdChatMessage = await CreateChatMessageAsync(personalChatMessageService, chatAction.ChatMessage);

                    await _personalChatMessageHelper.RequestMessageAsync(chatAction.ChatMessage.PersonalChatId, createdChatMessage);

                    if (chatAction.ChatMessage.Type == Chat.Domain.Enums.MessageType.Default)
                    {
                        await IncreaseUnreadMessageCountAsync(personalChat, chatAction.ChatMessage.AppUserId, personalChatService);
                        await _personalChatUnreadMessageHelper.RequestUnreadMessagesAsync(chatAction.ChatMessage.PersonalChatId, chatAction.ChatMessage.AppUserId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
                    }

                    break;
                case ChatMessageActionState.Read:
                    if (chatAction.ChatMessage.Type == Chat.Domain.Enums.MessageType.Default)
                    {
                        await ReadMessageAsync(personalChatMessageService, chatAction.ChatMessage.Id);

                        await DecreaseUnreadMessageCountAsync(personalChat, chatAction.ChatMessage.AppUserId, personalChatService);

                        await _personalChatUnreadMessageHelper.RequestUnreadMessagesAsync(chatAction.ChatMessage.PersonalChatId, chatAction.ChatMessage.AppUserId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
                    }

                    break;
                case ChatMessageActionState.Edited:
                    break;
                default:
                    break;
            }

            await _personalChatMessageHelper.DisconnectFromHubAsync();
            await _personalChatUnreadMessageHelper.DisconnectFromHubAsync();
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

    private static async Task<PersonalChatMessageDto> CreateChatMessageAsync(IPersonalChatMessageService chatMessageService, PersonalChatMessageDto chatMessage)
    {
        var createdChatMessage = await chatMessageService.CreateAsync(chatMessage);

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

    private static async Task ReadMessageAsync(IPersonalChatMessageService chatMessageService, int chatMessageId)
    {
        await chatMessageService.UpdateChatMessageAsync(chatMessageId, null, Chat.Domain.Enums.MessageStatus.Read, null);
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
