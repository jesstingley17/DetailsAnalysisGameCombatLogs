using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Enums;
using Chat.Domain.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatMessageConsumer> logger, 
    IServiceScopeFactory serviceScopeFactory, IMapper mapper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
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
            _logger.LogError(ex, "Consume Group Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var groupChatMessageService = scope.ServiceProvider.GetService<IGroupChatMessageService>();
            ArgumentNullException.ThrowIfNull(groupChatMessageService, nameof(groupChatMessageService));

            var groupChatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
            ArgumentNullException.ThrowIfNull(groupChatUserService, nameof(groupChatUserService));

            var groupChatMessageHub = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(groupChatMessageHub, nameof(groupChatMessageHub));

            var groupChatUnreadMessageHub = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(groupChatUnreadMessageHub, nameof(groupChatUnreadMessageHub));

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            await groupChatMessageHub.ConnectToHubAsync($"{_hubs.Server}{_hubs.GroupChatMessagesAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await groupChatMessageHub.JoinRoomAsync(chatAction.ChatMessage.GroupChatId);

            await groupChatUnreadMessageHub.ConnectToHubAsync($"{_hubs.Server}{_hubs.GroupChatUnreadMessageAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await groupChatUnreadMessageHub.JoinRoomAsync(chatAction.ChatMessage.GroupChatId);

            switch (chatAction.State)
            {
                case ChatMessageActionState.Created:
                    var createdChatMessage = await CreateChatMessageAsync(groupChatMessageService, chatAction.ChatMessage);

                    await groupChatMessageHub.RequestMessageAsync(chatAction.ChatMessage.GroupChatId, chatAction.ChatMessage);

                    if (chatAction.ChatMessage.Type == MessageType.Default)
                    {
                        await IncreaseUnreadMessageCountAsync(groupChatUserService, chatAction.ChatMessage.GroupChatId, chatAction.InitiatorGroupChatUserId);
                        await groupChatUnreadMessageHub.RequestUnreadMessagesAsync(chatAction.ChatMessage.GroupChatId);
                    }

                    break;
                case ChatMessageActionState.Read:
                    if (chatAction.ChatMessage.Type == MessageType.Default)
                    {
                        await DecreaseUnreadMessageCountAsync(groupChatUserService, groupChatMessageService, chatAction.InitiatorGroupChatUserId, chatAction.ChatMessage.GroupChatId, chatAction.ChatMessage.Id);
                        await ReadMessageAsync(groupChatUserService, groupChatMessageService, chatAction.InitiatorGroupChatUserId, chatAction.ChatMessage);

                        await groupChatUnreadMessageHub.RequestUnreadMessagesAsync(chatAction.ChatMessage.GroupChatId);
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
            _logger.LogError(ex, "Create group chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message from Kafka Consumer (topic: {Topic}) failed. Group chat {Id} not found.", KafkaTopics.GroupChatMessage, ex.GroupChatId);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message from Kafka Consumer (topic: {Topic}) failed. Group chat user {Id} not found.", KafkaTopics.GroupChatMessage, ex.UserId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat message from Kafka Consumer (topic: {Topic}).", KafkaTopics.GroupChatMessage);
        }
    }

    private async Task<GroupChatMessageDto> CreateChatMessageAsync(IGroupChatMessageService chatMessageService, GroupChatMessageModel chatMessage)
    {
        var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
        var createdChatMessage = await chatMessageService.CreateAsync(map);

        return createdChatMessage;
    }

    private static async Task IncreaseUnreadMessageCountAsync(IGroupChatUserService groupChatUserService, int groupChatId, string groupChatUserId)
    {
        var groupChatUsers = await groupChatUserService.FindAllAsync(groupChatId);

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != groupChatUserId).ToList();

        foreach (var groupChatUser in otherGroupChatUsers)
        {
            groupChatUser.UnreadMessages++;

            await groupChatUserService.UpdateAsync(groupChatUser);
        }
    }

    private static async Task ReadMessageAsync(IGroupChatUserService chatUserService, IGroupChatMessageService chatMessageService, string initiatorGroupChatUserId, GroupChatMessageModel chatMessage)
    {
        await chatUserService.MarkAsReadAsync(initiatorGroupChatUserId, chatMessage.Id);

        await chatMessageService.ReadMessagesLessThanAsync(chatMessage.GroupChatId, chatMessage.Id);
    }

    private static async Task DecreaseUnreadMessageCountAsync(IGroupChatUserService groupChatUserService, IGroupChatMessageService chatMessageService, string chatUserId, int chatId, int chatMessageId)
    {
        var groupChatUser = await groupChatUserService.GetByIdAsync(chatUserId);
        ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

        var countReadUnreadMessage = await chatMessageService.CountReadUnreadMessagesAsync(chatId, chatMessageId, groupChatUser.LastReadMessageId ?? 0);

        if (countReadUnreadMessage <= 0 || groupChatUser.UnreadMessages == 0)
        {
            return;
        }

        groupChatUser.UnreadMessages -= countReadUnreadMessage;

        await groupChatUserService.UpdateAsync(groupChatUser);
    }
}
