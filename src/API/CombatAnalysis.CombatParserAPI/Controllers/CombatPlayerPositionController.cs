using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController(IQueryService<CombatPlayerPositionDto> queryCombatPlayerPosition) : ControllerBase
{
    private readonly IQueryService<CombatPlayerPositionDto> _queryCombatPlayerPosition = queryCombatPlayerPosition;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayerPositions = await _queryCombatPlayerPosition.GetByParamAsync(nameof(CombatPlayerPositionModel.CombatId), combatId, cancellationToken);

        return Ok(combatPlayerPositions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatPlayerPosition = await _queryCombatPlayerPosition.GetByIdAsync(id, cancellationToken);

        return Ok(combatPlayerPosition);
    }
}
