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
public class PersonalChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public PersonalChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/count/{chatId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/getByChatId?chatId={chatId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/getMoreByChatId?chatId={chatId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel message)
    {
        var responseMessage = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var personalChatMessage = await responseMessage.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            return Ok(personalChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel message)
    {
        var responseMessage = await _httpClient.PutAsync("PersonalChatMessage", JsonContent.Create(message));
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

    [HttpDelete("{messageId:int:min(1)}")]
    public async Task<IActionResult> Delete(int messageId)
    {
        var responseMessage = await _httpClient.DeletAsync($"PersonalChatMessage/{messageId}");
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

    [HttpDelete("deleteByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> DeleteByChatId(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"PersonalChatMessage/findByChatId/{chatId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
            foreach (var item in messages)
            {
                await Delete(item.Id);
            }

            return Ok();
        }

        return BadRequest();
    }
}
