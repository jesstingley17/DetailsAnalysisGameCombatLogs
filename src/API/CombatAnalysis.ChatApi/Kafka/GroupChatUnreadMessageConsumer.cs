using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
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
            var chatTransaction = scope.ServiceProvider.GetService<IChatTransactionService>();
            ArgumentNullException.ThrowIfNull(chatTransaction, nameof(chatTransaction));

            await ExecuteAsync(scope, chatTransaction, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, IChatTransactionService chatTransaction, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            await chatTransaction.BeginTransactionAsync();

            var groupChatUser = scope.ServiceProvider.GetService<IServiceTransaction<GroupChatUserDto, string>>();
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatUnreadMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

            var unreadGroupChatMessageService = scope.ServiceProvider.GetService<IService<UnreadGroupChatMessageDto, int>>();
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessageService, nameof(unreadGroupChatMessageService));

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatUnreadMessageAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await chatHubHelper.JoinRoomAsync(chatAction.ChatId);

            if (chatAction.State == (int)ChatMessageActionState.Created)
            {
                await IncreaseCountAsync(chatHubHelper, chatAction, groupChatUser, unreadGroupChatMessageService);
            }
            else if (chatAction.State == (int)ChatMessageActionState.Read)
            {
                await DecreaseCountAsync(scope, chatHubHelper, chatAction, groupChatUser, unreadGroupChatMessageService);
            }

            await chatTransaction.CommitTransactionAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (Exception)
        {
            await chatTransaction.RollbackTransactionAsync();

            throw;
        }
    }

    private static async Task IncreaseCountAsync(IChatHubHelper chatHubHelper, GroupChatUnreadMessageAction chatAction, IServiceTransaction<GroupChatUserDto, string> groupChatUserService, IService<UnreadGroupChatMessageDto, int> unreadGroupChatMessageService)
    {
        var groupChatUsers = await groupChatUserService.GetByParamAsync(nameof(GroupChatUnreadMessageAction.ChatId), chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != chatAction.GroupChatUserId).ToList();
        ArgumentNullException.ThrowIfNull(otherGroupChatUsers, nameof(otherGroupChatUsers));

        foreach (var groupChatUser in otherGroupChatUsers)
        {
            groupChatUser.UnreadMessages++;

            var rowsAffected = await groupChatUserService.UpdateAsync(groupChatUser);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            var createdUnreadGroupChatMessage = await unreadGroupChatMessageService.CreateAsync(new UnreadGroupChatMessageDto
            {
                GroupChatUserId = groupChatUser.Id,
                GroupChatMessageId = chatAction.MessageId,
            });
            ArgumentNullException.ThrowIfNull(createdUnreadGroupChatMessage, nameof(createdUnreadGroupChatMessage));
        }

        await chatHubHelper.RequestUnreadMessagesAsync(chatAction.ChatId, chatAction.GroupChatUserId);
    }

    private async Task DecreaseCountAsync(IServiceScope scope, IChatHubHelper chatHubHelper, GroupChatUnreadMessageAction chatAction, IServiceTransaction<GroupChatUserDto, string> groupChatUserService, IService<UnreadGroupChatMessageDto, int> unreadGroupChatMessageServic)
    {
        var groupChatUsers = await groupChatUserService.GetByParamAsync(nameof(GroupChatUnreadMessageAction.ChatId), chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var meAsGroupChatUser = groupChatUsers.FirstOrDefault(x => x.Id == chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(meAsGroupChatUser, nameof(meAsGroupChatUser));

        if (meAsGroupChatUser.UnreadMessages == 0)
        {
            return;
        }

        meAsGroupChatUser.UnreadMessages--;

        var rowsAffected = await groupChatUserService.UpdateAsync(meAsGroupChatUser);
        ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

        var getMyUnreadGroupChatMessages = await unreadGroupChatMessageServic.GetByParamAsync(nameof(UnreadGroupChatMessageDto.GroupChatUserId), chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(getMyUnreadGroupChatMessages, nameof(getMyUnreadGroupChatMessages));

        if (getMyUnreadGroupChatMessages.Any())
        {
            var currentUnreadMessage = getMyUnreadGroupChatMessages.FirstOrDefault(m => m.GroupChatMessageId == chatAction.MessageId);
            if (currentUnreadMessage != null)
            {
                rowsAffected = await unreadGroupChatMessageServic.DeleteAsync(currentUnreadMessage.Id);
                ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));
            }
        }

        var chatMessageHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatMessageHubHelper, nameof(chatMessageHubHelper));

        await chatMessageHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatMessagesAddress}", chatAction.RefreshToken, chatAction.AccessToken);
        await chatMessageHubHelper.JoinRoomAsync(chatAction.ChatId);
        await chatMessageHubHelper.SendMessageAlreadyRead(chatAction.ChatId, chatAction.MessageId);

        await chatHubHelper.RequestUnreadMessagesAsync(chatAction.ChatId, chatAction.GroupChatUserId);
    }
}
