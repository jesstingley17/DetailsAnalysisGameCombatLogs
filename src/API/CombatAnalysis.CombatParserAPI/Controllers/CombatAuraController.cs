using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatAuraController(IQueryService<CombatAuraDto> queryService, IMutationServiceBatch<CombatAuraDto> mutationService,
    IMapper mapper, ILogger<CombatAuraController> logger) : ControllerBase
{
    private readonly IQueryService<CombatAuraDto> _queryService = queryService;
    private readonly IMutationServiceBatch<CombatAuraDto> _mutationService = mutationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatAuraController> _logger = logger;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var combatAuras = await _queryService.GetByParamAsync(nameof(CombatAuraModel.CombatId), combatId);

        return Ok(combatAuras);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatAura = await _queryService.GetByIdAsync(id);

        return Ok(combatAura);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatAuraModel combatAura)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CombatAura create received: {@CombatAura}", combatAura);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatAuraDto>(combatAura);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat aura.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var entityDeleted = await _mutationService.DeleteAsync(id);
            if (!entityDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
