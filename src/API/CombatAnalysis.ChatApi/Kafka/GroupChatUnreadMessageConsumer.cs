using Chat.Application.Interfaces;
using Chat.Domain.Enums;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatUnreadMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatUnreadMessageConsumer> logger,
    IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatUnreadMessage, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly ILogger<GroupChatUnreadMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

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
            _logger.LogError(ex, "Consume Group Chat Unread Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatUnreadMessage, ex.ParamName);
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

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatUnreadMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatUnreadMessageAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await chatHubHelper.JoinRoomAsync(chatAction.ChatId);

            if (chatAction.State == ChatMessageActionState.Created)
            {
                await IncreaseCountAsync(chatHubHelper, chatAction, groupChatUserService);
            }
            else if (chatAction.State == ChatMessageActionState.Read)
            {
                await DecreaseCountAsync(scope, chatHubHelper, chatAction, groupChatUserService, groupChatMessageService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update group chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat user from Kafka Consumer (topic: {Topic}) failed. Group chat user {Id} not found.", KafkaTopics.GroupChatMessage, ex.EntityId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update group chat user from Kafka Consumer (topic: {Topic}) failed. Chat user not found or modified.", KafkaTopics.GroupChatMessage);
        }
    }

    private static async Task IncreaseCountAsync(IChatHubHelper chatHubHelper, GroupChatUnreadMessageAction chatAction, IGroupChatUserService groupChatUserService)
    {
        var groupChatUsers = await groupChatUserService.FindAllAsync(chatAction.ChatId);

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != chatAction.GroupChatUserId).ToList();

        foreach (var groupChatUser in otherGroupChatUsers)
        {
            groupChatUser.UnreadMessages++;

            await groupChatUserService.UpdateAsync(groupChatUser);
        }

        await chatHubHelper.RequestUnreadMessagesAsync(chatAction.ChatId, chatAction.GroupChatUserId);
    }

    private async Task DecreaseCountAsync(IServiceScope scope, IChatHubHelper chatHubHelper, GroupChatUnreadMessageAction chatAction, IGroupChatUserService groupChatUserService, IGroupChatMessageService groupChatMessageService)
    {
        var groupChatUsers = await groupChatUserService.FindAllAsync(chatAction.ChatId);

        var meAsGroupChatUser = groupChatUsers.FirstOrDefault(x => x.Id == chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(meAsGroupChatUser, nameof(meAsGroupChatUser));

        var message = await groupChatMessageService.GetByIdAsync(chatAction.MessageId);

        message.Status = MessageStatus.Read;

        await groupChatMessageService.UpdateAsync(message);

        if (meAsGroupChatUser.UnreadMessages == 0)
        {
            return;
        }

        meAsGroupChatUser.UnreadMessages--;

        await groupChatUserService.UpdateAsync(meAsGroupChatUser);

        var chatMessageHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatMessageHubHelper, nameof(chatMessageHubHelper));

        await chatMessageHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatMessagesAddress}", chatAction.RefreshToken, chatAction.AccessToken);
        await chatMessageHubHelper.JoinRoomAsync(chatAction.ChatId);
        await chatMessageHubHelper.SendMessageReadAsync(chatAction.ChatId, chatAction.MessageId);

        await chatHubHelper.RequestUnreadMessagesAsync(chatAction.ChatId, chatAction.GroupChatUserId);
    }
}
