using CombatAnalysis.NotificationAPI.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace CombatAnalysis.NotificationAPI.Helpers;

public class ChatHubHelper(ILogger<ChatHubHelper> logger) : IChatHubHelper
{
    private readonly ILogger<ChatHubHelper> _logger = logger;

    private HubConnection? _notificationHubConnection;

    public async Task ConnectToHubAsync(string hubURL, string refreshToken, string accessToken)
    {
        try
        {
            _notificationHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinRoomAsync(string appUserId)
    {
        if (_notificationHubConnection == null)
        {
            return;
        }

        await _notificationHubConnection.SendAsync("JoinRoom", appUserId);
    }

    public async Task RequestNotificationAsync(int notificationId, int chatId)
    {
        if (_notificationHubConnection == null)
        {
            return;
        }

        await _notificationHubConnection.SendAsync("RequestNotification", notificationId, chatId);
    }

    public async Task RequestRecipientNotifications(string recipientId)
    {
        if (_notificationHubConnection == null)
        {
            return;
        }

        await _notificationHubConnection.SendAsync("RequestRecipientNotifications", recipientId);
    }

    private static async Task<HubConnection> CreateHubConnectionAsync(string hubURL, string refreshToken, string accessToken)
    {
        var cookieContainer = new CookieContainer();

        cookieContainer.Add(new Uri(hubURL), new Cookie("RefreshToken", refreshToken));
        cookieContainer.Add(new Uri(hubURL), new Cookie("AccessToken", accessToken));

        var hub = new HubConnectionBuilder()
            .WithUrl(hubURL, options =>
            {
                options.Cookies = cookieContainer;
            })
            .Build();

        await hub.StartAsync();

        return hub;
    }
}