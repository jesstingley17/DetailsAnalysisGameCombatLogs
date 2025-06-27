using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.WebApp.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UnreadGroupChatMessageController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UnreadGroupChatMessageController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("find")]
    public async Task<IActionResult> Find(int messageId, string groupChatUserId)
    {
        var responseMessage = await _httpClient.GetAsync("UnreadGroupChatMessage");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var groupChatMessagesCount = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();
        var myGroupChatMessagesCount = groupChatMessagesCount?.Where(x => x.GroupChatMessageId == messageId && x.GroupChatUserId == groupChatUserId).FirstOrDefault();

        return Ok(myGroupChatMessagesCount);
    }

    [HttpGet("findByMessageId/{messageId:int:min(1)}")]
    public async Task<IActionResult> FindByMessageId(int messageId)
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
}
