using CombatAnalysis.ChatApi.Models.Kafka;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Services;

public class PersonalChatConsumerService : KafkaConsumerServiceBase
{
    private readonly ILogger<PersonalChatConsumerService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PersonalChatConsumerService(IConfiguration configuration, ILogger<PersonalChatConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
        : base(configuration, "personal-chat", logger)
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

            var messageCreatedModel = result.Message.Value.Deserialize<MessageCreatedModel>();
            ArgumentNullException.ThrowIfNull(messageCreatedModel);

            var messagesCount = await chatMessageCountService.GetByParamAsync(nameof(MessageCreatedModel.ChatId), messageCreatedModel.ChatId);
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
