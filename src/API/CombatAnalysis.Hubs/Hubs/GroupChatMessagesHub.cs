using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka.Actions;
using CombatAnalysis.Hubs.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

public class GroupChatMessagesHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatMessagesHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;

    public GroupChatMessagesHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<GroupChatMessagesHub> logger, IKafkaProducerService<string, string> kafkaProducer)
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join chat to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task SendMessage(string message, int chatId, int type, string groupChatUserId, string username)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentOutOfRangeException.ThrowIfLessThan(type, 0, nameof(type));
            ArgumentNullException.ThrowIfNullOrEmpty(groupChatUserId, nameof(groupChatUserId));
            ArgumentNullException.ThrowIfNullOrEmpty(username, nameof(username));

            var chatAction = JsonSerializer.Serialize(new GroupChatMessageAction
            {
                Message = new GroupChatMessageModel
                {
                    Message = message,
                    ChatId = chatId,
                    Type = type,
                    GroupChatUserId = groupChatUserId,
                    Username = username
                },
                State = (int)ChatMessageActionState.Created,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task RequestMessage(int chatId, GroupChatMessageModel message)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Requests messages failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task SendMessageHasBeenRead(int chatMessageId, string myselfInChatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatMessageId, nameof(chatMessageId));
            ArgumentNullException.ThrowIfNullOrEmpty(myselfInChatId, nameof(myselfInChatId));

            var response = await _httpClient.GetAsync($"GroupChatMessage/{chatMessageId}");
            response.EnsureSuccessStatusCode();

            var chatMessage = await response.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            if (chatMessage.Status == 2)
            {
                return;
            }

            var chatAction = JsonSerializer.Serialize(new GroupChatUnreadMessageAction
            {
                ChatId = chatMessage.ChatId,
                MessageId = chatMessage.Id,
                GroupChatUserId = myselfInChatId,
                State = (int)ChatMessageActionState.Read,
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatUnreadMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message has been read failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task MessageAlreadyRead(int chatId, int chatMessageId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentOutOfRangeException.ThrowIfZero(chatMessageId, nameof(chatMessageId));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessageHasBeenRead", chatMessageId);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(int room)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(room, 1, nameof(room));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Leave from room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }
}
