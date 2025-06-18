using CombatAnalysis.ChatApi.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace CombatAnalysis.ChatApi.Helpers;

public class ChatHubHelper(ILogger<ChatHubHelper> logger) : IChatHubHelper
{
    private readonly ILogger<ChatHubHelper> _logger = logger;

    private HubConnection? _chatHubConnection;

    public async Task ConnectToHubAsync(string hubURL, string refreshToken, string accessToken)
    {
        try
        {
            _chatHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
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

    public async Task JoinRoomAsync(int chatId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task RequestUnreadMessagesAsync(int chatId, string appUserId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("RequestUnreadMessages", chatId, appUserId);
    }

    public async Task SendMessageAlreadyRead(int chatId, int chatMessageId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("MessageAlreadyRead", chatId, chatMessageId);
    }

    public async Task RequestsChats(int chatId, string appUserId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("RequestJoinedUser", chatId, appUserId);
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
