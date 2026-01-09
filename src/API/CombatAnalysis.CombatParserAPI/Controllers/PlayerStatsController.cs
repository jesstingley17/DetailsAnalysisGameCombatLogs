using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerStatsController(IQueryService<CombatPlayerStatsDto> queryService) : ControllerBase
{
    private readonly IQueryService<CombatPlayerStatsDto> _queryService = queryService;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var playerStats = await _queryService.GetByParamAsync(nameof(CombatPlayerStatsModel.CombatPlayerId), combatPlayerId, cancellationToken);
        var firstPlayerStats = playerStats.SingleOrDefault();

        return Ok(firstPlayerStats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var playerParseInfo = await _queryService.GetByIdAsync(id, cancellationToken);

        return Ok(playerParseInfo);
    }
}
