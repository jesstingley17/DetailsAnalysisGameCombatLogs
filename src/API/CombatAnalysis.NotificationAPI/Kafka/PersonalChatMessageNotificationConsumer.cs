using CombatAnalysis.NotificationAPI.Consts;
using CombatAnalysis.NotificationAPI.Enums;
using CombatAnalysis.NotificationAPI.Interfaces;
using CombatAnalysis.NotificationAPI.Kafka.Actions;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.NotificationAPI.Kafka;

public class PersonalChatMessageNotificationConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<PersonalChatMessageNotificationConsumer> logger,
    IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.PersonalChatMessage, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly ILogger<PersonalChatMessageNotificationConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var notificationService = scope.ServiceProvider.GetService<IService<NotificationDto, int>>();
            ArgumentNullException.ThrowIfNull(notificationService, nameof(notificationService));

            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

            var chatAction = kafkaData.Message.Value.Deserialize<PersonalChatMessageAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.NotificationAddress}", chatAction.RefreshToken, chatAction.AccessToken);
            await chatHubHelper.JoinRoomAsync(chatAction.InititatorId);

            if (chatAction.State == (int)ChatActionState.Created)
            {
                await CreateNotificationAsync(chatHubHelper, chatAction, notificationService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Notification API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private static async Task CreateNotificationAsync(IChatHubHelper chatHubHelper, PersonalChatMessageAction chatAction, IService<NotificationDto, int> notificationService)
    {
        var notification = new NotificationDto
        {
            InitiatorId = chatAction.ChatId.ToString(),
            InitiatorName = chatAction.InititatorUsername,
            RecipientId = chatAction.RecipientId,
            Type = (int)NotificationType.PersonalChatMessage,
            Status = 0,
            CreatedAt = DateTime.UtcNow,
        };

        var createdNotification = await notificationService.CreateAsync(notification);
        ArgumentNullException.ThrowIfNull(createdNotification, nameof(createdNotification));

        await chatHubHelper.RequestNotificationAsync(createdNotification.Id, chatAction.ChatId);
    }
}
