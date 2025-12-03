using Chat.Application.Consts;
using CombatAnalysis.NotificationAPI.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.NotificationAPI.Helpers;

public class ChatHubHelper(ILogger<ChatHubHelper> logger, IOptions<KafkaSettings> kafkaSettings) : IChatHubHelper
{
    private readonly ILogger<ChatHubHelper> _logger = logger;
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;

    private HubConnection? _notificationHubConnection;

    public async Task ConnectToHubAsync(string hubURL, string accessToken)
    {
        try
        {
            _notificationHubConnection = await CreateHubConnectionAsync(hubURL, accessToken);
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

    public async Task DisconnectFromHubAsync()
    {
        if (_notificationHubConnection != null)
        {
            await _notificationHubConnection.StopAsync();
            await _notificationHubConnection.DisposeAsync();
        }
    }

    private async Task<HubConnection> CreateHubConnectionAsync(string hubURL, string accessToken)
    {
        var cookieContainer = new CookieContainer();

        cookieContainer.Add(new Uri(hubURL), new Cookie("AccessToken", accessToken) { Expires = DateTime.UtcNow.AddMinutes(_kafkaSettings.AccessTokenExpiresMins) });

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