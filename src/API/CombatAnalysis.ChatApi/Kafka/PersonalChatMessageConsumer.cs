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
            var chatMessageCountService = scope.ServiceProvider.GetService<IService<PersonalChatDto, int>>();
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
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while updating Personal Chat Message count: {ex.Message}");
        }
    }

    private static async Task IncreaseCountAsync(PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> chatMessageCountService)
    {
        var personalChat = await chatMessageCountService.GetByIdAsync(chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(personalChat);

        if (chatAction.AppUserId == personalChat.CompanionId)
        {
            personalChat.InitiatorUnreadMessages++;
        }
        else
        {
            personalChat.CompanionUnreadMessages++;
        }

        var affectedRows = await chatMessageCountService.UpdateAsync(personalChat);
        ArgumentOutOfRangeException.ThrowIfLessThan(affectedRows, 1, nameof(affectedRows));
    }
    private static async Task DecreaseCountAsync(PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> chatMessageCountService)
    {
        var personalChat = await chatMessageCountService.GetByIdAsync(chatAction.ChatId);
        ArgumentNullException.ThrowIfNull(personalChat);

        if (chatAction.AppUserId == personalChat.CompanionId)
        {
            personalChat.CompanionUnreadMessages--;
        }
        else
        {
            personalChat.InitiatorUnreadMessages--;
        }

        var affectedRows =await chatMessageCountService.UpdateAsync(personalChat);
        ArgumentOutOfRangeException.ThrowIfLessThan(affectedRows, 1, nameof(affectedRows));
    }
}
