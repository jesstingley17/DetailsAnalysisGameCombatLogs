using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerPositionController(IQueryService<CombatPlayerPositionDto> queryCombatPlayerPosition, IMutationService<CombatPlayerPositionDto> mutationCombatPlayerService,
    IMapper mapper, ILogger<CombatPlayerPositionController> logger) : ControllerBase
{
    private readonly IQueryService<CombatPlayerPositionDto> _queryCombatPlayerPosition = queryCombatPlayerPosition;
    private readonly IMutationService<CombatPlayerPositionDto> _mutationCombatPlayerService = mutationCombatPlayerService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatPlayerPositionController> _logger = logger;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var combatPlayerPositions = await _queryCombatPlayerPosition.GetByParamAsync(nameof(CombatPlayerPositionModel.CombatId), combatId);

        return Ok(combatPlayerPositions);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatPlayerPosition = await _queryCombatPlayerPosition.GetByIdAsync(id);

        return Ok(combatPlayerPosition);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatPlayerPositionModel combatPlayerPosition)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CombatPlayerPosition create received: {@CombatPlayerPosition}", combatPlayerPosition);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatPlayerPositionDto>(combatPlayerPosition);
            var createdItem = await _mutationCombatPlayerService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat player position.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _mutationCombatPlayerService.DeleteAsync(id);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
