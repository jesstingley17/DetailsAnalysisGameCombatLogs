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
public class GroupChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatController> _logger;

    public GroupChatController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<GroupChatController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync("GroupChat");
            responseMessage.EnsureSuccessStatusCode();

            var groupChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatModel>>();

            return Ok(groupChats);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get all group chats failed. User should be authorize to get all group chats");
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get all group chats failed: received unsuccessful request");
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChat = await responseMessage.Content.ReadFromJsonAsync<GroupChatModel>();

            return Ok(groupChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get group chat {Id} failed. User should be authorize to get group chat", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get group chat {Id} failed: received unsuccessful request", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, GroupChatModel chat)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"GroupChat/{id}", JsonContent.Create(chat));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update group chat {Id} failed. User should be authorize to update group chat", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update group chat {Id} failed. Chat not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"GroupChat/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete group chat {Id} failed. User should be authorize to delete group chat", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete group chat {Id} failed. Chat not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
