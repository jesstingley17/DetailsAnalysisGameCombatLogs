using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController(IQueryService<CombatAuraDto> queryService) : ControllerBase
{
    private readonly IQueryService<CombatAuraDto> _queryService = queryService;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId, CancellationToken cancellationToken)
    {
        var combatAuras = await _queryService.GetByParamAsync(nameof(CombatAuraModel.CombatId), combatId, cancellationToken);

        return Ok(combatAuras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var combatAura = await _queryService.GetByIdAsync(id, cancellationToken);

        return Ok(combatAura);
    }
}
