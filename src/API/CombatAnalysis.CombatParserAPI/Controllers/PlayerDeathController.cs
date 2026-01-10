using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController(IPlayerInfoPaginationService<CombatPlayerDeathDto> service) : ControllerBase
{
    private readonly IPlayerInfoPaginationService<CombatPlayerDeathDto> _playerInfoService = service;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(damageTakens);
    }
}
