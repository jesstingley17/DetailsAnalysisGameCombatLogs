using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class PersonalChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<PersonalChatMessageConsumer> logger,
    IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly ILogger<PersonalChatMessageConsumer> _logger = logger;
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
            _logger.LogError(ex, "Consume Personal Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PersonalChatMessage, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var personalChatService = scope.ServiceProvider.GetService<IService<PersonalChatDto, int>>();
            ArgumentNullException.ThrowIfNull(personalChatService, nameof(personalChatService));

            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            var personalChat = await personalChatService.GetByIdAsync(chatAction.ChatId);
            ArgumentNullException.ThrowIfNull(personalChat, nameof(personalChat));

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.PersonalChatUnreadMessageAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await chatHubHelper.JoinRoomAsync(personalChat.Id);

            if (chatAction.State == ChatMessageActionState.Created)
            {
                await IncreaseCountAsync(chatHubHelper, personalChat, chatAction, personalChatService);
            }
            else if (chatAction.State == ChatMessageActionState.Read)
            {
                await DecreaseCountAsync(chatHubHelper, personalChat, chatAction, personalChatService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PersonalChatMessage, ex.ParamName);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat {Id} not found.", KafkaTopics.PersonalChatMessage, ex.EntityId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat not found or modified.", KafkaTopics.PersonalChatMessage);
        }
    }

    private static async Task IncreaseCountAsync(IChatHubHelper chatHubHelper, PersonalChatDto personalChat, PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> personalChatService)
    {
        if (chatAction.InititatorId == personalChat.CompanionId)
        {
            personalChat.InitiatorUnreadMessages++;
        }
        else
        {
            personalChat.CompanionUnreadMessages++;
        }

        await personalChatService.UpdateAsync(personalChat);

        await chatHubHelper.RequestUnreadMessagesAsync(personalChat.Id, chatAction.InititatorId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
    }

    private static async Task DecreaseCountAsync(IChatHubHelper chatHubHelper, PersonalChatDto personalChat, PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> personalChatService)
    {
        if (chatAction.InititatorId == personalChat.CompanionId)
        {
            personalChat.CompanionUnreadMessages--;
        }
        else
        {
            personalChat.InitiatorUnreadMessages--;
        }

        await personalChatService.UpdateAsync(personalChat);

        await chatHubHelper.RequestUnreadMessagesAsync(personalChat.Id, chatAction.InititatorId == personalChat.CompanionId ? personalChat.CompanionId : personalChat.InitiatorId);
    }
}
