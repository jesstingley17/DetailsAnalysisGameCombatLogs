using CombatAnalysis.ChatApi.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace CombatAnalysis.ChatApi.Helpers;

public class ChatHubHelper(ILogger<ChatHubHelper> logger) : IChatHubHelper
{
    private readonly ILogger<ChatHubHelper> _logger = logger;

    private HubConnection? _chatUnreadMessagesHubConnection;

    public async Task ConnectToUnreadMessageHubAsync(string hubURL, string refreshToken, string accessToken)
    {
        try
        {
            _chatUnreadMessagesHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
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

    public async Task JoinChatRoomAsync(int chatId)
    {
        if (_chatUnreadMessagesHubConnection == null)
        {
            return;
        }

        await _chatUnreadMessagesHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task RequestUnreadMessagesAsync(int chatId, string appUserId)
    {
        if (_chatUnreadMessagesHubConnection == null)
        {
            return;
        }

        await _chatUnreadMessagesHubConnection.SendAsync("RequestUnreadMessages", chatId, appUserId);
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
