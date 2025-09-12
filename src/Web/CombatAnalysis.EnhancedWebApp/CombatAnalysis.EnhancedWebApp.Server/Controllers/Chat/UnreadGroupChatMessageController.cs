using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UnreadGroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<UnreadGroupChatMessageController> _logger;

    public UnreadGroupChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<UnreadGroupChatMessageController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("find")]
    public async Task<IActionResult> Find(int messageId, string groupChatUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("UnreadGroupChatMessage");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();
            var myGroupChatMessagesCount = groupChatMessagesCount?.Where(x => x.GroupChatMessageId == messageId && x.GroupChatUserId == groupChatUserId).FirstOrDefault();

            return Ok(myGroupChatMessagesCount);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find unread group chat message for chat user {GroupChatUserId}. User should be authorize to find unread group chat message for chat user", groupChatUserId);
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Find unread group chat message for chat user {GroupChatUserId}. The specified parameters are incorrect", groupChatUserId);
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find unread group chat message for chat user {GroupChatUserId}: received unsuccessful request", groupChatUserId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findByMessageId/{messageId:int:min(1)}")]
    public async Task<IActionResult> FindByMessageId(int messageId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"UnreadGroupChatMessage/findByMessageId/{messageId}");
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else if (responseMessage.IsSuccessStatusCode)
            {
                var unredMessages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();

                return Ok(unredMessages);
            }

            return BadRequest();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find unread group chat message by message {MessageId}. User should be authorize to find unread group chat message by message", messageId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find unread group chat message by message {MessageId}: received unsuccessful request", messageId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
