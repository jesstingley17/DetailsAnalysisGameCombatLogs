using Chat.Application.Interfaces;
using Chat.Domain.Enums;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using Confluent.Kafka;
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
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
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

            if (chatAction.State == (int)ChatMessageActionState.Created)
            {
                await IncreaseCountAsync(chatHubHelper, chatAction, groupChatUserService);
            }
            else if (chatAction.State == (int)ChatMessageActionState.Read)
            {
                await DecreaseCountAsync(scope, chatHubHelper, chatAction, groupChatUserService, groupChatMessageService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    private static async Task IncreaseCountAsync(IChatHubHelper chatHubHelper, GroupChatUnreadMessageAction chatAction, IGroupChatUserService groupChatUserService)
    {
        var groupChatUsers = await groupChatUserService.FindAllAsync(chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != chatAction.GroupChatUserId).ToList();
        ArgumentNullException.ThrowIfNull(otherGroupChatUsers, nameof(otherGroupChatUsers));

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
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var meAsGroupChatUser = groupChatUsers.FirstOrDefault(x => x.Id == chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(meAsGroupChatUser, nameof(meAsGroupChatUser));

        var message = await groupChatMessageService.GetByIdAsync(chatAction.MessageId);
        ArgumentNullException.ThrowIfNull(message, nameof(message));

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
        await chatMessageHubHelper.SendMessageAlreadyRead(chatAction.ChatId, chatAction.MessageId);

        await chatHubHelper.RequestUnreadMessagesAsync(chatAction.ChatId, chatAction.GroupChatUserId);
    }
}
