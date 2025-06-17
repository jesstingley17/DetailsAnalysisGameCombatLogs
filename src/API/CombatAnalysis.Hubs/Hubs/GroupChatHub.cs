using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka.Actions;
using CombatAnalysis.Hubs.Models;
using CombatAnalysis.Hubs.Models.Containers;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

public class GroupChatHub(IHttpClientHelper httpClient, ILogger<GroupChatHub> logger, IKafkaProducerService<string, string> kafkaProducer) : Hub
{
    private readonly IHttpClientHelper _httpClient = httpClient;
    private readonly ILogger<GroupChatHub> _logger = logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

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
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task CreateGroupChat(GroupChatContainerModel container)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));

            var chatAction = JsonSerializer.Serialize(new GroupChatAction
            {
                Chat = container.GroupChat,
                Rules = container.GroupChatRules,
                User = container.GroupChatUser,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChat, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RequestJoinedUser(int chatId, string appUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var response = await _httpClient.GetAsync($"GroupChatUser/findUserInChat?chatId={chatId}&appUserId={appUserId}");
            response.EnsureSuccessStatusCode();

            var groupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            await Clients.Group(appUserId).SendAsync("ReceiveJoinedUser", groupChatUser);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request joined users failed: Parameter '{ParamName}' was null.", ex.ParamName);
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

    public async Task AddUserToChat(GroupChatUserModel groupChatUser)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            var response = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser));
            response.EnsureSuccessStatusCode();

            var createdGroupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            ArgumentNullException.ThrowIfNull(createdGroupChatUser, nameof(createdGroupChatUser));

            await Clients.Caller.SendAsync("ReceiveAddedUserToChat", createdGroupChatUser);
            await Clients.Group(groupChatUser.AppUserId).SendAsync("ReceiveAddedUserToChat", createdGroupChatUser);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Add user to chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
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
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
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
