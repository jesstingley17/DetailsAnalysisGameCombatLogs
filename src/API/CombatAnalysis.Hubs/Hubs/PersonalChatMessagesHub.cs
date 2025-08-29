using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka.Actions;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

public class PersonalChatMessagesHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatMessagesHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;

    public PersonalChatMessagesHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<PersonalChatMessagesHub> logger, IKafkaProducerService<string, string> kafkaProducer)
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
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

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

    public async Task SendMessage(string message, int chatId, string creatorId, string username, string companionId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, nameof(message));
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            ArgumentNullException.ThrowIfNullOrEmpty(creatorId, nameof(creatorId));
            ArgumentNullException.ThrowIfNullOrEmpty(username, nameof(username));
            ArgumentNullException.ThrowIfNullOrEmpty(companionId, nameof(companionId));

            var personalMessage = new PersonalChatMessageModel
            {
                Username = username,
                Message = message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
                Status = 0,
                ChatId = chatId,
                AppUserId = creatorId
            };

            var responseMessage = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(personalMessage));
            responseMessage.EnsureSuccessStatusCode();

            var createdMessage = await responseMessage.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            ArgumentNullException.ThrowIfNull(createdMessage, nameof(createdMessage));

            var chatAction = JsonSerializer.Serialize(new PersonalChatMessageAction
            {
                ChatId = createdMessage.ChatId,
                InititatorUsername = username,
                InititatorId = creatorId,
                RecipientId = companionId,
                State = (int)ChatActionState.Created,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, createdMessage.Id.ToString(), chatAction);

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", createdMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request unread messages failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
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

    public async Task SendMessageHasBeenRead(int chatMessageId, string meId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"PersonalChatMessage/{chatMessageId}");
            response.EnsureSuccessStatusCode();

            var chatMessage = await response.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            if (chatMessage.Status == 2)
            {
                return;
            }

            chatMessage.Status = 2;

            response = await _httpClient.PutAsync("PersonalChatMessage", JsonContent.Create(chatMessage));
            response.EnsureSuccessStatusCode();

            var chatAction = JsonSerializer.Serialize(new PersonalChatMessageAction
            {
                ChatId = chatMessage.ChatId,
                InititatorId = meId,
                State = (int)ChatActionState.Read,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, chatMessage.Id.ToString(), chatAction);

            await Clients.Caller.SendAsync("ReceiveMessageHasBeenRead", chatMessage.Id);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message has been read failed: Parameter '{ParamName}' was null.", ex.ParamName);
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
