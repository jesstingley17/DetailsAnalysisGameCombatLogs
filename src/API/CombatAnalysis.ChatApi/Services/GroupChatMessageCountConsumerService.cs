using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Models.Kafka;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Services;

public class GroupChatMessageCountConsumerService : KafkaConsumerServiceBase
{
    private readonly ILogger<GroupChatMessageCountConsumerService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GroupChatMessageCountConsumerService(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageCountConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
        : base(kafkaSettings, KafkaTopics.GroupChat, logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> result, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatMessageCountService = scope.ServiceProvider.GetService<IService<GroupChatMessageCountDto, int>>();

            ArgumentNullException.ThrowIfNull(chatMessageCountService);

            var chatAction = result.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction);

            var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
            var otherGroupCounts = messagesCount.Where(x => x.GroupChatUserId != chatAction.GroupChatUserId);
            ArgumentNullException.ThrowIfNull(otherGroupCounts);

            foreach (var groupCount in otherGroupCounts)
            {
                groupCount.Count++;
                await chatMessageCountService.UpdateAsync(groupCount);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message count was failed: ${ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while updating Personal Chat Message count: {ex.Message}");
        }
    }
}
