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
public class GroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatMessageController> _logger;

    public GroupChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<GroupChatMessageController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/count/{chatId}");
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get group chat messages count by chat {ChatId} failed. User should be authorize to get group chat messages count", chatId);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get group chat messages count by chat {ChatId} failed. Something wrong during getting count of chat messages.", chatId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int page, int pageSize)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/getByChatId?chatId={chatId}&page={page}&pageSize={pageSize}");
            responseMessage.EnsureSuccessStatusCode();

            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(messages);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get group chat messages for chat {ChatId} failed. User should be authorize to get group chat messages.", chatId);

            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get group chat messages for chat {ChatId} failed. Something wrong during getting chat.", chatId);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel message)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message));
            responseMessage.EnsureSuccessStatusCode();

            var groupChatMessage = await responseMessage.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            return Ok(groupChatMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create group chat message failed. User should be authorize to create group chat message.");

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Create group chat message failed. Some Entities not found.");

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create group chat message failed. Something wrong during creating group chat message.");

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, GroupChatMessageModel message)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"GroupChatMessage/{id}", JsonContent.Create(message));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. User should be authorize to update chat group chat message.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. Group chat message not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. Something wrong during deleting group chat message.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"GroupChatMessage/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete group chat message {Id} failed. User should be authorize to delete group chat message.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. Group chat message not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete group chat message {Id} failed. Something wrong during deleting group chat message.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
