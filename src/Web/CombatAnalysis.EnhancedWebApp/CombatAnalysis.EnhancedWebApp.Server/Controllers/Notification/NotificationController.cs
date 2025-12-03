using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Notification;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public NotificationController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Notification;
    }

    [HttpGet("getByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetByRecipientId(string recipientId)
    {
        var responseMessage = await _httpClient.GetAsync($"Notification/getByRecipientId/{recipientId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var recipientNotifications = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<NotificationModel>>();

            return Ok(recipientNotifications);
        }

        return BadRequest();
    }

    [HttpGet("getUnreadByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetUnreadByRecipientId(string recipientId)
    {
        var responseMessage = await _httpClient.GetAsync($"Notification/getUnreadByRecipientId/{recipientId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var recipientNotifications = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<NotificationModel>>();

            return Ok(recipientNotifications);
        }

        return BadRequest();
    }
}
