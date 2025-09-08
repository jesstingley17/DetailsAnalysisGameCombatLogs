using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CombatAnalysis.Core.Helpers;

internal class ChatHubHelper(IMemoryCache memoryCache, ILogger<ChatHubHelper> logger) : IChatHubHelper
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ILogger<ChatHubHelper> _logger = logger;

    private HubConnection? _chatHubConnection;
    private HubConnection? _chatMessagesHubConnection;
    private HubConnection? _unreadMessagesHubConnection;

    public async Task ConnectToChatHubAsync(string hubURL)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _chatHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinChatRoomAsync(string appUserId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("JoinRoom", appUserId);
    }

    public async Task ConnectToChatMessagesHubAsync(string hubURL)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _chatMessagesHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinChatMessagesRoomAsync(int chatId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task SendMessageAsync(string message, int chatId, string appUserId, string username, int type = -1)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        if (type > -1)
        {
            await _chatMessagesHubConnection.SendAsync("SendMessage", message, chatId, type, appUserId, username);
        }
        else
        {
            await _chatMessagesHubConnection.SendAsync("SendMessage", message, chatId, appUserId, username);
        }
    }

    public async Task ConnectToUnreadMessagesHubAsync(string hubURL)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _unreadMessagesHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinUnreadMessagesRoomAsync(int chatId)
    {
        if (_unreadMessagesHubConnection == null)
        {
            return;
        }

        await _unreadMessagesHubConnection.SendAsync("JoinRoom", chatId);
    }

    public void SubscribeToChat(Action<PersonalChatModel> callback)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        _chatHubConnection.On("ReceivePersonalChat", callback);
    }

    public void SubscribeUnreadMessagesUpdated(Action<int, string, int> receiveUnreadMessageAction)
    {
        if (_unreadMessagesHubConnection == null)
        {
            return;
        }

        _unreadMessagesHubConnection.On("ReceiveUnreadMessage", receiveUnreadMessageAction);
    }

    public void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action)
        where T : class
    {
        if (_chatMessagesHubConnection == null || _unreadMessagesHubConnection == null)
        {
            return;
        }

        _chatMessagesHubConnection.On("ReceiveMessage", action);
    }

    public async Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("SendMessageHasBeenRead", messageId, appUserId);
    }

    public void SubscribeReceiveMessageHasBeenRead<T>(Action<T> action)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        _chatMessagesHubConnection.On("ReceiveMessageHasBeenRead", action);
    }

    public async Task LeaveFromChatRoomAsync(int chatId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("LeaveFromRoom", chatId);
    }

    public async Task LeaveFromUnreadMessageRoomAsync(int chatId)
    {
        if (_unreadMessagesHubConnection == null)
        {
            return;
        }

        await _unreadMessagesHubConnection.SendAsync("LeaveFromRoom", chatId);
    }

    public async Task StopAsync()
    {
        if (_unreadMessagesHubConnection != null)
        {
            await _unreadMessagesHubConnection.StopAsync();
        }

        if (_chatMessagesHubConnection != null)
        {
            await _chatMessagesHubConnection.StopAsync();
        }
    }

    private static async Task<HubConnection> CreateHubConnectionAsync(string hubURL, string refreshToken, string accessToken)
    {
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Uri(hubURL), new Cookie(nameof(MemoryCacheValue.RefreshToken), refreshToken));
        cookieContainer.Add(new Uri(hubURL), new Cookie(nameof(MemoryCacheValue.AccessToken), accessToken));

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
