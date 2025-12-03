using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatLogController(IQueryService<CombatLogDto> queryCombatLogService, IMutationService<CombatLogDto> mutationCombatLogService,
    IMapper mapper, ILogger<CombatLogController> logger) : ControllerBase
{
    private readonly IQueryService<CombatLogDto> _queryCombatLogService = queryCombatLogService;
    private readonly IMutationService<CombatLogDto> _mutationCombatLogService = mutationCombatLogService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatLogController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combatLogs = await _queryCombatLogService.GetAllAsync();

        return Ok(combatLogs);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatLog = await _queryCombatLogService.GetByIdAsync(id);

        return Ok(combatLog);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatLogModel combatLog)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CombatLog create received: {@CombatLog}", combatLog);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatLogDto>(combatLog);
            var createdItem = await _mutationCombatLogService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat log.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CombatLogModel combatLog)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CombatLog create received: {@CombatLog}", combatLog);

                return ValidationProblem(ModelState);
            }

            if (id != combatLog.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<CombatLogDto>(combatLog);
            await _mutationCombatLogService.UpdateAsync(map);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var entityDeleted = await _mutationCombatLogService.DeleteAsync(id);
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
