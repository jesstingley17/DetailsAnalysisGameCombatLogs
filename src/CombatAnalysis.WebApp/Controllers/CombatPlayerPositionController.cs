using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CombatPlayerPositionController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.CombatParser;
    }

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var response = await _httpClient.GetAsync($"CombatPlayerPosition/getByCombatId/{combatId}");
        var combatPlayerPositions = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerPositionModel>>();

        return Ok(combatPlayerPositions);
    }
}
