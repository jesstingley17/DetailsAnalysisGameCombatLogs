using Chat.Application.Consts;
using Chat.Application.Security;
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

public class NotificationConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<NotificationConsumer> logger,
    IServiceScopeFactory serviceScopeFactory) : KafkaConsumerBase(kafkaSettings, KafkaTopics.Notification, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private readonly ILogger<NotificationConsumer> _logger = logger;
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

            var notificationAction = kafkaData.Message.Value.Deserialize<NotificationAction>();
            ArgumentNullException.ThrowIfNull(notificationAction, nameof(notificationAction));

            var accessToken = AesEncryption.Decrypt(notificationAction.AccessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.NotificationAddress}", accessToken);
            await chatHubHelper.JoinRoomAsync(notificationAction.RecipientId);

            if (notificationAction.State == (int)NotificationActionState.Read)
            {
                await ReadNotificationAsync(chatHubHelper, notificationAction, notificationService);
            }
            else if (notificationAction.State == (int)NotificationActionState.Remove)
            {
                await RemoveNotificationAsync(chatHubHelper, notificationAction, notificationService);
            }
            else if (notificationAction.State == (int)NotificationActionState.ReadAll)
            {
                await ReadRecipientNotificationsAsync(chatHubHelper, notificationAction, notificationService);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Notification API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    private static async Task ReadNotificationAsync(IChatHubHelper chatHubHelper, NotificationAction notificationAction, IService<NotificationDto, int> notificationService)
    {
        var notification = await notificationService.GetByIdAsync(notificationAction.NotificationId);
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        notification.Status = (int)NotificationStatus.Read;
        notification.ReadAt = DateTime.UtcNow;
        await notificationService.UpdateAsync(notification);

        await chatHubHelper.RequestRecipientNotifications(notificationAction.RecipientId);
    }

    private static async Task RemoveNotificationAsync(IChatHubHelper chatHubHelper, NotificationAction notificationAction, IService<NotificationDto, int> notificationService)
    {
        var notification = await notificationService.GetByIdAsync(notificationAction.NotificationId);
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        notification.Status = (int)NotificationStatus.Read;
        await notificationService.UpdateAsync(notification);

        await chatHubHelper.RequestRecipientNotifications(notificationAction.RecipientId);
    }

    private static async Task ReadRecipientNotificationsAsync(IChatHubHelper chatHubHelper, NotificationAction notificationAction, IService<NotificationDto, int> notificationService)
    {
        var notifications = await notificationService.GetByParamAsync(n => n.RecipientId, notificationAction.RecipientId);
        ArgumentNullException.ThrowIfNull(notifications, nameof(notifications));

        foreach (var notification in notifications)
        {
            notification.Status = (int)NotificationStatus.Read;
            await notificationService.UpdateAsync(notification);
        }

        await chatHubHelper.RequestRecipientNotifications(notificationAction.RecipientId);
    }
}
