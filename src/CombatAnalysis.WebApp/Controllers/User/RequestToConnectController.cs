using CombatAnalysis.WebApp.Attributes;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.WebApp.Controllers.User;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class RequestToConnectController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public RequestToConnectController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.User;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

            return Ok(requestToConnect);
        }

        return BadRequest();
    }

    [HttpGet("searchByOwnerId/{userId}")]
    public async Task<IActionResult> SearchByOwnerId(string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByOwnerId/{userId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();

            return Ok(requestsToConnect);
        }

        return BadRequest();
    }

    [HttpGet("searchByToUserId/{userId}")]
    public async Task<IActionResult> SearchByToUserId(string userId)
    {
        var responseMessage = await _httpClient.GetAsync($"RequestToConnect/searchByToUserId/{userId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();

            return Ok(requestsToConnect);
        }

        return BadRequest();
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(string initiatorId, string companionId)
    {
        var responseMessage = await _httpClient.GetAsync("RequestToConnect");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (!responseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        var allRequestsToConnect = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RequestToConnectModel>>();
        var requestsToConnectToUser = allRequestsToConnect.Where(x => x.ToAppUserId == initiatorId && x.AppUserId == companionId).ToList();
        if (requestsToConnectToUser.Any())
        {
            return Ok(true);
        }

        var requestsToConnectToOwner = allRequestsToConnect.Where(x => x.ToAppUserId == companionId && x.AppUserId == initiatorId).ToList();
        if (requestsToConnectToOwner.Any())
        {
            return Ok(true);
        }

        return Ok(false);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestToConnectModel model)
    {
        var responseMessage = await _httpClient.PostAsync("RequestToConnect", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var requestToConnect = await responseMessage.Content.ReadFromJsonAsync<RequestToConnectModel>();

            return Ok(requestToConnect);
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"RequestToConnect/{id}");
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
