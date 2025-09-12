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
public class GroupChatUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatUserController> _logger;

    public GroupChatUserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<GroupChatUserController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);

        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get chat user {Id} failed. User should be authorize to get chat user", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get chat user {Id} failed: received unsuccessful request", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findMeInChat")]
    public async Task<IActionResult> Find(int chatId, string appUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findMeInChat?chatId={chatId}&appUserId={appUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var meInChat = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(meInChat);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find chat user by app user {AppUserId} for chat {ChatId} failed. User should be authorize to find chat user by app user id", appUserId, chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Find chat user by app user {AppUserId} for chat {ChatId} failed. The specified parameters are incorrect", appUserId, chatId);
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find chat user by app user {AppUserId} for chat {ChatId} failed: received unsuccessful request", appUserId, chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int chatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByChatId/{chatId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find all users in chat {ChatId} failed. User should be authorize to find all users in chat", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find all users in chat {ChatId} failed: received unsuccessful request", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }


    [HttpGet("findByUserId/{appUserId:minlength(8)}")]
    public async Task<IActionResult> FindByUserId(string appUserId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"GroupChatUser/findByUserId/{appUserId}");
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUsers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();

            return Ok(groupChatUsers);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Find chat users by app user {AppUserId} failed. User should be authorize to find chat users by app user id", appUserId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Find chat users by app user {AppUserId} failed: received unsuccessful request", appUserId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatUserModel user)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(user));
            responseMessage.EnsureSuccessStatusCode();

            var groupChatUser = await responseMessage.Content.ReadFromJsonAsync<GroupChatUserModel>();

            return Ok(groupChatUser);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create chat user failed. User should be authorize to create chat user");
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Create chat user failed. The specified parameters are incorrect");
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create chat user failed: received unsuccessful request");
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"GroupChatUser/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete chat user {Id} failed. User should be authorize to delete chat user", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete chat user {Id} failed. Chat not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
