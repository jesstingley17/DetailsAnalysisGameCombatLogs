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
public class GroupChatRulesController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatRulesController> _logger;

    public GroupChatRulesController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<GroupChatRulesController> logger)
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
            var responseMessage = await _httpClient.GetAsync("GroupChatRules");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatsRules = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatRulesModel>>();

            return Ok(groupChatsRules);

        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get all chat rules failed. User should be authorize to get all chat rules");
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get all chat rules failed: received unsuccessful request");
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatRules/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatRules = await responseMessage.Content.ReadFromJsonAsync<GroupChatRulesModel>();

            return Ok(groupChatRules);

        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get chat rules {Id} failed. User should be authorize to get chat rules", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get chat rules {Id} failed: received unsuccessful request", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int chatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatRules/findByChatId/{chatId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatsRules = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatRulesModel>>();
            var groupChatRules = groupChatsRules?.FirstOrDefault();

            return Ok(groupChatRules);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get chat rules by chat {ChatId} failed. User should be authorize to get chat rules by chat", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get chat rules by chat {ChatId} failed: received unsuccessful request", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatRulesModel rules)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("GroupChatRules", JsonContent.Create(rules));
            responseMessage.EnsureSuccessStatusCode();

            var groupChatRules = await responseMessage.Content.ReadFromJsonAsync<GroupChatRulesModel>();

            return Ok(groupChatRules);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create chat rules failed. User should be authorize to create chat rules");
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Create chat rules failed. The specified parameters are incorrect");
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create chat rules failed: received unsuccessful request");
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, GroupChatRulesModel rules)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"GroupChatRules/{id}", JsonContent.Create(rules));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update chat rules {Id} failed. User should be authorize to update chat rules", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update chat rules {Id} failed. Chat not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"GroupChatRules/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete chat rules {Id} failed. User should be authorize to delete chat rules", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete chat rules {Id} failed. Chat not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
