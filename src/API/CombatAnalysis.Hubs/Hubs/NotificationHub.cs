using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka.Actions;
using CombatAnalysis.Hubs.Models.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

public class NotificationHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<NotificationHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;

    public NotificationHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<NotificationHub> logger, IKafkaProducerService<string, string> kafkaProducer)
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Notification;
    }

    public async Task JoinRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.AddToGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join chat to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RequestNotification(int notificationId, int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(notificationId, nameof(notificationId));

            var response = await _httpClient.GetAsync($"Notification/{notificationId}");
            response.EnsureSuccessStatusCode();

            var notification = await response.Content.ReadFromJsonAsync<NotificationModel>();
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            await Clients.Group(notification.RecipientId).SendAsync("ReceiveNotification", notification);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request notification failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task ReadRecipientNotifications(string recipientId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(recipientId, nameof(recipientId));

            var notificationAction = JsonSerializer.Serialize(new NotificationAction
            {
                RecipientId = recipientId,
                State = (int)NotificationActionState.ReadAll,
                When = DateTime.UtcNow.ToString(),
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.Notification, recipientId, notificationAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request notification failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
    }

    public async Task ReadNotification(int notificationId, string recipientId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(notificationId, 1, nameof(notificationId));

            var notificationAction = JsonSerializer.Serialize(new NotificationAction
            {
                NotificationId = notificationId,
                RecipientId = recipientId,
                State = (int)NotificationActionState.Read,
                When = DateTime.UtcNow.ToString(),
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.Notification, notificationId.ToString(), notificationAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
    }

    public async Task RemoveNotification(int notificationId, string recipientId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(notificationId, 1, nameof(notificationId));

            var notificationAction = JsonSerializer.Serialize(new NotificationAction
            {
                NotificationId = notificationId,
                RecipientId = recipientId,
                State = (int)NotificationActionState.Remove,
                When = DateTime.UtcNow.ToString(),
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.Notification, notificationId.ToString(), notificationAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
    }

    public async Task RequestRecipientNotifications(string recipientId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(recipientId, nameof(recipientId));

            await Clients.Group(recipientId).SendAsync("ReceiveRequestRecipientNotifications");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request notification failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Leave from room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogError(exception, exception.Message);
        }

        return base.OnDisconnectedAsync(exception);
    }
}
