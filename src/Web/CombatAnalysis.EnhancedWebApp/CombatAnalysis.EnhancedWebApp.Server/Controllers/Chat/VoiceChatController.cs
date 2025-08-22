using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class VoiceChatController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public VoiceChatController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("VoiceChat");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<VoiceChatModel>>();

            return Ok(groupChats);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"VoiceChat/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(groupChat);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(VoiceChatModel chat)
    {
        var responseMessage = await _httpClient.PostAsync("VoiceChat", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChat = await responseMessage.Content.ReadFromJsonAsync<VoiceChatModel>();

            return Ok(groupChat);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(VoiceChatModel chat)
    {
        var responseMessage = await _httpClient.PutAsync("VoiceChat", JsonContent.Create(chat));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var responseMessage = await _httpClient.DeletAsync($"VoiceChat/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }
}
