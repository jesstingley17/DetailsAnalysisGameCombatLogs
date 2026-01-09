using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SpecializationScoreController(ISpecializationScoreService service) : ControllerBase
{
    private readonly ISpecializationScoreService _service = service;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, CancellationToken cancellationToken)
    {
        var score = await _service.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(score);
    }
}
