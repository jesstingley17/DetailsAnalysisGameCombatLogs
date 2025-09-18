using CombatAnalysis.ChatApi.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace CombatAnalysis.ChatApi.Helpers;

internal class ChatHubHelper : IChatHubHelper
{
    private HubConnection? _chatHubConnection;

    public async Task ConnectToHubAsync(string hubURL, string refreshToken, string accessToken)
    {
        _chatHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
    }

    public async Task JoinRoomAsync(int chatId)
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task RequestUnreadMessagesAsync(int chatId, string appUserId)
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("RequestUnreadMessages", chatId, appUserId);
    }

    public async Task RequestUnreadMessagesAsync(int chatId)
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("RequestUnreadMessages", chatId);
    }

    public async Task SendMessageReadAsync(int chatId, int chatMessageId)
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("SendMessageRead", chatId, chatMessageId);
    }

    public async Task RequestsChatsAsync(int chatId, string appUserId)
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("RequestJoinedUser", chatId, appUserId);
    }

    public async Task RequestMessageAsync<T>(int chatId, T message)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

        await _chatHubConnection.SendAsync("RequestMessage", chatId, message);
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
