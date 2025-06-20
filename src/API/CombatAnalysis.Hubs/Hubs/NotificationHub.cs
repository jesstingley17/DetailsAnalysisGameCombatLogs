using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using CombatAnalysis.Hubs.Models.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace CombatAnalysis.Hubs.Hubs;

public class NotificationHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<NotificationHub> _logger;
    private readonly Cluster _cluster;

    public NotificationHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<NotificationHub> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cluster = cluster.Value;
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

            _httpClient.APIUrl = _cluster.Chat;
            response = await _httpClient.GetAsync($"PersonalChat/{chatId}");
            response.EnsureSuccessStatusCode();

            var personalChat = await response.Content.ReadFromJsonAsync<PersonalChatModel>();
            ArgumentNullException.ThrowIfNull(personalChat, nameof(personalChat));

            var targetUserId = personalChat.InitiatorId == notification.InitiatorId
                ? personalChat.CompanionId
                : personalChat.InitiatorId;

            await Clients.Group(targetUserId).SendAsync("ReceiveNotification", notification);
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
