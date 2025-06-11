using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageConsumer> logger, IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
{
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData);

            using var scope = _serviceScopeFactory.CreateScope();
            var chatTransaction = scope.ServiceProvider.GetService<IChatTransactionService>();
            ArgumentNullException.ThrowIfNull(chatTransaction);

            await ExecuteAsync(scope, chatTransaction, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Some argument receavied as null while consume Chat API data: ${ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while consume Chat API data: {ex.Message}");
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, IChatTransactionService chatTransaction, ConsumeResult<string, JsonDocument> kafkaData)
    {

        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData);

            await chatTransaction.BeginTransactionAsync();

            var chatMessageCountService = scope.ServiceProvider.GetService<IService<GroupChatMessageCountDto, int>>();
            ArgumentNullException.ThrowIfNull(chatMessageCountService);

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction);

            if (chatAction.State == (int)KafkaActionState.Created)
            {
                await IncreaseCountAsync(chatAction, chatMessageCountService);
            }
            else if (chatAction.State == (int)KafkaActionState.Read)
            {
                await DecreaseCountAsync(chatAction, chatMessageCountService);
            }

            await chatTransaction.CommitTransactionAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Some argument receavied as null while consume Chat API data: ${ex.Message}");

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while consume Chat API data: {ex.Message}");

            await chatTransaction.RollbackTransactionAsync();
        }
    }

    private static async Task IncreaseCountAsync(GroupChatMessageAction chatAction, IService<GroupChatMessageCountDto, int> chatMessageCountService)
    {
        var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        var otherGroupCounts = messagesCount.Where(x => x.GroupChatUserId != chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(otherGroupCounts);

        foreach (var groupCount in otherGroupCounts)
        {
            groupCount.Count++;
            await chatMessageCountService.UpdateAsync(groupCount);
        }
    }
    private static async Task DecreaseCountAsync(GroupChatMessageAction chatAction, IService<GroupChatMessageCountDto, int> chatMessageCountService)
    {
        var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        var myCounts = messagesCount.FirstOrDefault(x => x.GroupChatUserId == chatAction.GroupChatUserId);
        ArgumentNullException.ThrowIfNull(myCounts);

        myCounts.Count--;
        await chatMessageCountService.UpdateAsync(myCounts);
    }
}
