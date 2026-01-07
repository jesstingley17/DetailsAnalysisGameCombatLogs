using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController(IMutationServiceBatch<HealDoneDto> mutationService, IPlayerInfoPaginationService<HealDoneDto> playerInfoService,
    ICountService<HealDoneDto> countService, IGeneralFilterService<HealDoneDto> filterService,
    IMapper mapper, ILogger<HealDoneController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<HealDoneDto> _mutationService = mutationService;
    private readonly IPlayerInfoPaginationService<HealDoneDto> _playerInfoService = playerInfoService;
    private readonly ICountService<HealDoneDto> _countService = countService;
    private readonly IGeneralFilterService<HealDoneDto> _filterService = filterService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<HealDoneController> _logger = logger;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var healDones = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        return Ok(healDones);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var count = await _countService.CountByCombatPlayerIdAsync(combatPlayerId);

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId)
    {
        var uniqueTargets = await _filterService.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

        return Ok(uniqueTargets);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        var healDones = await _filterService.GetByTargetAsync(combatPlayerId, target, page, pageSize);

        return Ok(healDones);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target)
    {
        var count = await _filterService.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId)
    {
        var uniqueSpells = await _filterService.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize)
    {
        var healDones = await _filterService.GetBySpellAsync(combatPlayerId, spell, page, pageSize);

        return Ok(healDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HealDoneModel healDone)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid HealDone create received: {@HealDone}", healDone);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<HealDoneDto>(healDone);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create heal done.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
