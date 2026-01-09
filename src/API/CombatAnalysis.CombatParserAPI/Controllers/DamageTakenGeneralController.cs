using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController(IPlayerInfoService<DamageTakenGeneralDto> playerInfoService) : ControllerBase
{
    private readonly IPlayerInfoService<DamageTakenGeneralDto> _playerInfoService = playerInfoService;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damageTakenGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(damageTakenGenerals);
    }
}
