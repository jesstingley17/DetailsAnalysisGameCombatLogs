using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SpecializationScoreController(ISpecializationScoreService service, ILogger<SpecializationScoreController> logger) : ControllerBase
{
    private readonly ISpecializationScoreService _service = service;
    private readonly ILogger<SpecializationScoreController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var score = await _service.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(score);
    }
}
