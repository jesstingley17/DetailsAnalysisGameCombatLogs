using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.User;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<UserController> _logger;

    public UserController(IOptions<Cluster> cluster, IHttpClientHelper httpClient, ILogger<UserController> logger)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.User;
        _logger = logger;
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var responseMessage = await _httpClient.GetAsync($"User/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return NoContent();
        }
        else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var user = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            return Ok(user);
        }

        return BadRequest();
    }

    [ServiceFilter(typeof(RequireAccessTokenAttribute))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AppUserModel user)
    {
        try
        {
            var responseMessage = await _httpClient.PutAsync($"User/{id}", JsonContent.Create(user));
            responseMessage.EnsureSuccessStatusCode();

            return NoContent();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError(ex, "Update account {Id} failed. User should be authorize to update account.", id);

            return Unauthorized();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Update account {Id} failed. Account not found.", id);

            return NotFound();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Update account {Id} failed. Something wrong during updating account.", id);

            return StatusCode((int)(ex.StatusCode ?? HttpStatusCode.InternalServerError), ex.Message);
        }
    }
}
