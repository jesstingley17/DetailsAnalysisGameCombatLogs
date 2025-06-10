using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Models.Kafka;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Services;

public class GroupChatMessageCountConsumerService(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageCountConsumerService> logger, IServiceScopeFactory serviceScopeFactory, 
    IChatTransactionService chatTransactionService) : KafkaConsumerServiceBase(kafkaSettings, KafkaTopics.GroupChat, logger)
{
    private readonly ILogger<GroupChatMessageCountConsumerService> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IChatTransactionService _chatTransactionService = chatTransactionService;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData);

            await _chatTransactionService.BeginTransactionAsync();

            using var scope = _serviceScopeFactory.CreateScope();
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

            await _chatTransactionService.CommitTransactionAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message count was failed: ${ex.Message}");

            await _chatTransactionService.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while updating Personal Chat Message count: {ex.Message}");


            await _chatTransactionService.RollbackTransactionAsync();
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
