using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController(IPlayerInfoService<HealDoneGeneralDto> playerInfoService) : ControllerBase
{
    private readonly IPlayerInfoService<HealDoneGeneralDto> _playerInfoService = playerInfoService;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var healDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(healDoneGenerals);
    }
}
