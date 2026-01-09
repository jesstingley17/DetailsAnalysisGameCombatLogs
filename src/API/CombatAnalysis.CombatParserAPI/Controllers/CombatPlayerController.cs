using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController(ICombatPlayerService service) : ControllerBase
{
    private readonly ICombatPlayerService _service = service;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayers = await _service.GetByCombatIdAsync(combatId, cancellationToken);

        return Ok(combatPlayers);
    }
}
