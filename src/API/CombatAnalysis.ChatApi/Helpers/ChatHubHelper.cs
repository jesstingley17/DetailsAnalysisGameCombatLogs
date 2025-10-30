using Chat.Application.Consts;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.ChatAPI.Helpers;

internal class ChatHubHelper(IOptions<Hubs> hubs, IOptions<KafkaSettings> kafkaSettings) : IChatHubHelper
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly KafkaSettings _kafkaSettings = kafkaSettings.Value;
    private HubConnection? _chatHubConnection;

    public async Task ConnectToHubAsync(string hubName, string accessToken)
    {
        _chatHubConnection = await CreateHubConnectionAsync($"{_hubs.Server}{hubName}", accessToken);
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
        try
        {
            ArgumentNullException.ThrowIfNull(_chatHubConnection, nameof(_chatHubConnection));

            await _chatHubConnection.SendAsync("RequestMessage", chatId, message);
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            throw;
        }
    }

    public async Task DisconnectFromHubAsync()
    {
        if (_chatHubConnection != null)
        {
            await _chatHubConnection.StopAsync();
            await _chatHubConnection.DisposeAsync();
        }
    }

    private async Task<HubConnection> CreateHubConnectionAsync(string hubUrl, string accessToken)
    {
        var cookieContainer = new CookieContainer();

        cookieContainer.Add(new Uri(hubUrl), new Cookie("AccessToken", accessToken) { Expires = DateTime.UtcNow.AddMinutes(_kafkaSettings.AccessTokenExpiresMins) });

        var hub = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.Cookies = cookieContainer;
            })
            .Build();

        await hub.StartAsync();

        return hub;
    }
}
