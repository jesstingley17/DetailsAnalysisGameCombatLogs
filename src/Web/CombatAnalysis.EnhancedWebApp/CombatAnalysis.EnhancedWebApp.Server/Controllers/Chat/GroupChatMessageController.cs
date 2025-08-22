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
public class GroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public GroupChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/count/{chatId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, string groupChatUserId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/getByChatId?chatId={chatId}&groupChatUserId={groupChatUserId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, string groupChatUserId,int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"GroupChatMessage/getMoreByChatId?chatId={chatId}&groupChatUserId={groupChatUserId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var messages = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();

            return Ok(messages);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel message)
    {
        var responseMessage = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var groupChatMessage = await responseMessage.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            return Ok(groupChatMessage);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel message)
    {
        var responseMessage = await _httpClient.PutAsync("GroupChatMessage", JsonContent.Create(message));
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


    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"GroupChatMessage/{id}");
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
