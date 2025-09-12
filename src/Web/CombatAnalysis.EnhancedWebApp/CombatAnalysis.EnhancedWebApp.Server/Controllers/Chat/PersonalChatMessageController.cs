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
public class PersonalChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatMessageController> _logger;

    public PersonalChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<PersonalChatMessageController> logger)
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
            var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/count/{chatId}");
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get personal chat messages count by chat {ChatId} failed. User should be authorize to get personal chat messages count", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get personal chat messages count by chat {ChatId} failed: received unsuccessful request", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/getByChatId?chatId={chatId}&pageSize={pageSize}");
            responseMessage.EnsureSuccessStatusCode();

            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

            return Ok(messages);

        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get personal chat messages for chat {ChatId} failed. User should be authorize to get personal chat messages", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Get personal chat messages for chat {ChatId} failed. The specified parameters are incorrect", chatId);
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get personal chat messages for chat {ChatId} failed: received unsuccessful request", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/getMoreByChatId?chatId={chatId}&offset={offset}&pageSize={pageSize}");
            responseMessage.EnsureSuccessStatusCode();

            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

            return Ok(messages);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Get more personal chat messages for chat {ChatId} failed. User should be authorize to get more personal chat messages", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Get more personal chat messages for chat {ChatId} failed. The specified parameters are incorrect", chatId);
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Get more personal chat messages for chat {ChatId} failed: received unsuccessful request", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel message)
    {
        try
        {
            var responseMessage = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message));
            responseMessage.EnsureSuccessStatusCode();

            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            return Ok(personalChatMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Create personal chat message failed. User should be authorize to create personal chat message");
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Create personal chat message failed. The specified parameters are incorrect");
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Create personal chat message failed: received unsuccessful request");
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, PersonalChatMessageModel message)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"PersonalChatMessage/{id}", JsonContent.Create(message));
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update personal chat message {Id} failed. User should be authorize to update chat personal chat message", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogError(ex, "Update personal chat message {Id} failed. The specified parameters are incorrect", id);
            return BadRequest();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update personal chat message {Id} failed: received unsuccessful request", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var responseMessage = await _httpClient.DeletAsync($"PersonalChatMessage/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete personal chat message {Id} failed. User should be authorize to delete personal chat message", id);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete personal chat message {Id} failed. Chat message not found or modified.", id);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }

    [HttpDelete("deleteByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> DeleteByChatId(int chatId)
    {
        try
        {
            var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/findByChatId/{chatId}");
            responseMessage.EnsureSuccessStatusCode();

            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>() ?? [];
            foreach (var item in messages)
            {
                await Delete(item.Id);
            }

            return Ok();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Delete personal chat message by chat {ChatId} failed. User should be authorize to delete personal chat message by chat", chatId);
            return Unauthorized();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Delete personal chat message by chat {ChatId} failed. Chat message not found or modified.", chatId);
            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
