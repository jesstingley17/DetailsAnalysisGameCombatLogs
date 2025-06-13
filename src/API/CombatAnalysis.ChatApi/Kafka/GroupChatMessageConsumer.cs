using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageConsumer> logger, IServiceScopeFactory serviceScopeFactory) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
{
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
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

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            if (chatAction.State == (int)KafkaActionState.Created)
            {
                await IncreaseCountAsync(chatAction, groupChatUser);
            }
            else if (chatAction.State == (int)KafkaActionState.Read)
            {
                await DecreaseCountAsync(chatAction, groupChatUser);
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

    private static async Task IncreaseCountAsync(GroupChatMessageAction chatAction, IServiceTransaction<GroupChatUserDto, string> groupChatUserService)
    {
        var groupChatUsers = await groupChatUserService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var otherGroupChatUsers = groupChatUsers.Where(x => x.Id != chatAction.GroupChatUserId).ToList();
        ArgumentNullException.ThrowIfNull(otherGroupChatUsers, nameof(otherGroupChatUsers));

        foreach (var groupChatUser in otherGroupChatUsers)
        {
            groupChatUser.UnreadMessages++;

            var rowsAffected = await groupChatUserService.UpdateAsync(groupChatUser);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));
        }
    }

    private static async Task DecreaseCountAsync(GroupChatMessageAction chatAction, IServiceTransaction<GroupChatUserDto, string> groupChatUserService)
    {
        var groupChatUsers = await groupChatUserService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

        var meInchat = groupChatUsers.FirstOrDefault(x => x.Id == chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(meInchat, nameof(meInchat));

        meInchat.UnreadMessages--;

        var rowsAffected = await groupChatUserService.UpdateAsync(meInchat);
        ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));
    }
}
