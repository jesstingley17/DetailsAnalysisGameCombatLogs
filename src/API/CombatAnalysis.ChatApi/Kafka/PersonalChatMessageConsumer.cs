using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class PersonalChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<PersonalChatMessageConsumer> logger, IServiceScopeFactory serviceScopeFactory) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
{
    private readonly ILogger<PersonalChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatMessageCountService = scope.ServiceProvider.GetService<IService<PersonalChatMessageCountDto, int>>();
            ArgumentNullException.ThrowIfNull(chatMessageCountService);

            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction);

            if (chatAction.State == (int)KafkaActionState.Created)
            {
                await IncreaseCountAsync(chatAction, chatMessageCountService);
            }
            else if (chatAction.State == (int)KafkaActionState.Read)
            {
                await DecreaseCountAsync(chatAction, chatMessageCountService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message count was failed: ${ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while updating Personal Chat Message count: {ex.Message}");
        }
    }

    private static async Task IncreaseCountAsync(PersonalChatMessageAction chatAction, IService<PersonalChatMessageCountDto, int> chatMessageCountService)
    {
        var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        var companionMessageCount = messagesCount.FirstOrDefault(x => x.AppUserId == chatAction.CompanionId);
        ArgumentNullException.ThrowIfNull(companionMessageCount);

        companionMessageCount.Count++;
        await chatMessageCountService.UpdateAsync(companionMessageCount);
    }
    private static async Task DecreaseCountAsync(PersonalChatMessageAction chatAction, IService<PersonalChatMessageCountDto, int> chatMessageCountService)
    {
        var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageAction.ChatId), chatAction.ChatId);
        var myCounts = messagesCount.FirstOrDefault(x => x.AppUserId == chatAction.AppUserId);
        ArgumentNullException.ThrowIfNull(myCounts);

        myCounts.Count--;
        await chatMessageCountService.UpdateAsync(myCounts);
    }
}
