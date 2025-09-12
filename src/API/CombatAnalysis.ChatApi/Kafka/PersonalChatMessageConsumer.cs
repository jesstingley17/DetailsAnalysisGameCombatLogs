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
            using var scope = _serviceScopeFactory.CreateScope();
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

            if (chatAction.State == (int)ChatMessageActionState.Created)
            {
                await IncreaseCountAsync(chatHubHelper, personalChat, chatAction, personalChatService);
            }
            else if (chatAction.State == (int)ChatMessageActionState.Read)
            {
                await DecreaseCountAsync(chatHubHelper, personalChat, chatAction, personalChatService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private static async Task IncreaseCountAsync(IChatHubHelper chatHubHelper, PersonalChatDto personalChat, PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> chatMessageCountService)
    {
        if (chatAction.InititatorId == personalChat.CompanionId)
        {
            personalChat.InitiatorUnreadMessages++;
        }
        else
        {
            personalChat.CompanionUnreadMessages++;
        }

        await chatMessageCountService.UpdateAsync(personalChat);

        await chatHubHelper.RequestUnreadMessagesAsync(personalChat.Id, chatAction.InititatorId == personalChat.CompanionId ? personalChat.InitiatorId : personalChat.CompanionId);
    }

    private static async Task DecreaseCountAsync(IChatHubHelper chatHubHelper, PersonalChatDto personalChat, PersonalChatMessageAction chatAction, IService<PersonalChatDto, int> chatMessageCountService)
    {
        if (chatAction.InititatorId == personalChat.CompanionId)
        {
            personalChat.CompanionUnreadMessages--;
        }
        else
        {
            personalChat.InitiatorUnreadMessages--;
        }

        await chatMessageCountService.UpdateAsync(personalChat);

        await chatHubHelper.RequestUnreadMessagesAsync(personalChat.Id, chatAction.InititatorId == personalChat.CompanionId ? personalChat.CompanionId : personalChat.InitiatorId);
    }
}
