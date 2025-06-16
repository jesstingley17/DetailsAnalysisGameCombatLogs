using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka.Actions;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

public class PersonalChatMessagesHub(IHttpClientHelper httpClient, ILogger<PersonalChatMessagesHub> logger, IKafkaProducerService<string, string> kafkaProducer) : Hub
{
    private readonly IHttpClientHelper _httpClient = httpClient;
    private readonly ILogger<PersonalChatMessagesHub> _logger = logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

    public async Task JoinRoom(int chatId)
    {
        try
        {
            var context = Context.GetHttpContext();
            ArgumentNullException.ThrowIfNull(context, nameof(context));

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

    public async Task SendMessage(string message, int chatId, string appUserId, string username)
    {
        try
        {
            var personalMessage = new PersonalChatMessageModel
            {
                Username = username,
                Message = message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
                Status = 0,
                ChatId = chatId,
                AppUserId = appUserId
            };

            var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(personalMessage));
            response.EnsureSuccessStatusCode();

            var createdMessage = await response.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            ArgumentNullException.ThrowIfNull(createdMessage, nameof(createdMessage));

            var chatAction = JsonSerializer.Serialize(new PersonalChatMessageAction
            {
                ChatId = createdMessage.ChatId,
                AppUserId = appUserId,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, createdMessage.Id.ToString(), chatAction);

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", createdMessage);
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
                AppUserId = meId,
                State = (int)KafkaActionState.Read,
                When = DateTime.UtcNow.ToString(),
                RefreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, chatMessage.Id.ToString(), chatAction);

            await Clients.Caller.SendAsync("ReceiveMessageHasBeenRead", chatMessage.Id);
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
