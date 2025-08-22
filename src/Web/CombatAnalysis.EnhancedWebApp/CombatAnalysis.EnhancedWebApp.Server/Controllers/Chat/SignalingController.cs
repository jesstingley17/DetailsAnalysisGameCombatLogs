using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public SignalingController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    [HttpGet("connected/{roomId}")]
    public async Task<IActionResult> GetConnectedUsers(int roomId)
    {
        var responseMessage = await _httpClient.GetAsync($"Signaling/connected/{roomId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var usersId = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>();

            return Ok(usersId);
        }

        return BadRequest();
    }
}
