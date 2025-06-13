using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

public class GroupChatUnreadMessageHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatUnreadMessageHub> _logger;

    public GroupChatUnreadMessageHub(IHttpClientHelper httpClient, ILogger<GroupChatUnreadMessageHub> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
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

    public async Task SendUnreadMessageUpdated(int chatId)
    {
        try
        {
            await Clients.OthersInGroup(chatId.ToString()).SendAsync("ReceiveUnreadMessageUpdated", chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task RequestUnreadMessages(int chatId, string meInChatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"GroupChatUser/{meInChatId}");
            response.EnsureSuccessStatusCode();

            var groupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveUnreadMessage", chatId, meInChatId, groupChatUser.UnreadMessages);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task LeaveFromRoom(int room)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
    }
}
