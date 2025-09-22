using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Interfaces;
using Chat.Application.Kafka.Actions;
using Chat.Application.Security;
using Chat.Domain.Enums;
using Chat.Domain.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Interfaces;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatMessageConsumer> logger, 
    IServiceScopeFactory serviceScopeFactory, IChatHubHelper groupChatMessageHelper, IChatHubHelper groupChatUnreadMessageHelper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IChatHubHelper _groupChatMessageHelper = groupChatMessageHelper;
    private readonly IChatHubHelper _groupChatUnreadMessageHelper = groupChatUnreadMessageHelper;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            using var scope = _serviceScopeFactory.CreateScope();

            await ExecuteActionAsync(scope, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Group Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
    }

    private async Task ExecuteActionAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var groupChatMessageService = scope.ServiceProvider.GetService<IGroupChatMessageService>();
            ArgumentNullException.ThrowIfNull(groupChatMessageService, nameof(groupChatMessageService));

            var groupChatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
            ArgumentNullException.ThrowIfNull(groupChatUserService, nameof(groupChatUserService));

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var accessToken = AesEncryption.Decrypt(chatAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            
            await _groupChatMessageHelper.ConnectToHubAsync(_hubs.GroupChatMessagesAddress, accessToken);
            await _groupChatMessageHelper.JoinRoomAsync(chatAction.ChatMessage.GroupChatId);

            await _groupChatUnreadMessageHelper.ConnectToHubAsync(_hubs.GroupChatUnreadMessageAddress, accessToken);
            await _groupChatUnreadMessageHelper.JoinRoomAsync(chatAction.ChatMessage.GroupChatId);

            switch (chatAction.State)
            {
                case ChatMessageActionState.Created:
                    var createdChatMessage = await CreateChatMessageAsync(groupChatMessageService, groupChatUserService, chatAction.ChatMessage);

                    await _groupChatMessageHelper.RequestMessageAsync(chatAction.ChatMessage.GroupChatId, createdChatMessage);

                    if (chatAction.ChatMessage.Type == MessageType.Default)
                    {
                        await IncreaseUnreadMessageCountAsync(groupChatUserService, chatAction.ChatMessage.GroupChatId, chatAction.InitiatorGroupChatUserId);
                        await _groupChatUnreadMessageHelper.RequestUnreadMessagesAsync(chatAction.ChatMessage.GroupChatId);
                    }

                    break;
                case ChatMessageActionState.Read:
                    if (chatAction.ChatMessage.Type == MessageType.Default)
                    {
                        await DecreaseUnreadMessageCountAsync(groupChatUserService, groupChatMessageService, chatAction.InitiatorGroupChatUserId, chatAction.ChatMessage.GroupChatId, chatAction.ChatMessage.Id);
                        await ReadMessageAsync(groupChatUserService, groupChatMessageService, chatAction.InitiatorGroupChatUserId, chatAction.ChatMessage.Id, chatAction.ChatMessage.GroupChatId);

                        await _groupChatUnreadMessageHelper.RequestUnreadMessagesAsync(chatAction.ChatMessage.GroupChatId);
                    }

                    break;
                case ChatMessageActionState.Edited:
                    break;
                default:
                    break;
            }

            await _groupChatMessageHelper.DisconnectFromHubAsync();
            await _groupChatUnreadMessageHelper.DisconnectFromHubAsync();
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

    private static async Task<GroupChatMessageDto> CreateChatMessageAsync(IGroupChatMessageService chatMessageService, IGroupChatUserService chatUserService, GroupChatMessageDto chatMessage)
    {
        var createdChatMessage = await chatMessageService.CreateAsync(chatMessage);

        await chatUserService.UpdateChatUserAsync(chatMessage.GroupChatUserId, createdChatMessage.Id);

        return createdChatMessage;
    }

    private static async Task IncreaseUnreadMessageCountAsync(IGroupChatUserService groupChatUserService, int groupChatId, string groupChatUserId)
    {
        var groupChatUsers = await groupChatUserService.FindAllAsync(groupChatId);

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != groupChatUserId).ToList();

        foreach (var groupChatUser in otherGroupChatUsers)
        {
            groupChatUser.UnreadMessages++;

            await groupChatUserService.UpdateChatUserAsync(groupChatUser.Id, null, groupChatUser.UnreadMessages);
        }
    }

    private static async Task ReadMessageAsync(IGroupChatUserService chatUserService, IGroupChatMessageService chatMessageService, string initiatorGroupChatUserId, int chatMessageId, int groupChatId)
    {
        await chatUserService.UpdateChatUserAsync(initiatorGroupChatUserId, chatMessageId);

        await chatMessageService.ReadMessagesLessThanAsync(groupChatId, chatMessageId);
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

        await groupChatUserService.UpdateChatUserAsync(groupChatUser.Id, null, groupChatUser.UnreadMessages);
    }
}
