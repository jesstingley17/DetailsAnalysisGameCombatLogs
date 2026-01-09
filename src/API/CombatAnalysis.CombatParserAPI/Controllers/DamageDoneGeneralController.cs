using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController(IPlayerInfoService<DamageDoneGeneralDto> playerInfoService) : ControllerBase
{
    private readonly IPlayerInfoService<DamageDoneGeneralDto> _playerInfoService = playerInfoService;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(damageDoneGenerals);
    }
}
