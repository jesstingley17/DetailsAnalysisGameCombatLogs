using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Models.Kafka;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Services;

public class PersonalChatMessageCountConsumerService : KafkaConsumerServiceBase
{
    private readonly ILogger<PersonalChatMessageCountConsumerService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PersonalChatMessageCountConsumerService(IOptions<KafkaSettings> kafkaSettings, ILogger<PersonalChatMessageCountConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
        : base(kafkaSettings, "personal-chat", logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> result, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatMessageCountService = scope.ServiceProvider.GetService<IService<PersonalChatMessageCountDto, int>>();

            ArgumentNullException.ThrowIfNull(chatMessageCountService);

            var messageCreatedModel = result.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(messageCreatedModel);

            var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(PersonalChatMessageAction.ChatId), messageCreatedModel.ChatId);
            var companionMessageCount = messagesCount.FirstOrDefault(x => x.AppUserId == messageCreatedModel.CompanionId);
            ArgumentNullException.ThrowIfNull(companionMessageCount);

            companionMessageCount.Count++;

            await chatMessageCountService.UpdateAsync(companionMessageCount);
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
