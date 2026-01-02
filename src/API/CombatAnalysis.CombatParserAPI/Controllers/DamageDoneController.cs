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
public class DamageDoneController(IMutationServiceBatch<DamageDoneDto> mutationService, IPlayerInfoService<DamageDoneDto> playerInfoService,
    ICountService<DamageDoneDto> countService, IGeneralFilterService<DamageDoneDto> filterService, IDamageFilterService damageFilterService,
    IMapper mapper, ILogger<DamageDoneController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<DamageDoneDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<DamageDoneDto> _playerInfoService = playerInfoService;
    private readonly ICountService<DamageDoneDto> _countService = countService;
    private readonly IGeneralFilterService<DamageDoneDto> _filterService = filterService;
    private readonly IDamageFilterService _damageFilterService = damageFilterService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DamageDoneController> _logger = logger;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var damageDones = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        return Ok(damageDones);
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

    [HttpGet("getDamageByEachTarget/{combatId}")]
    public async Task<IActionResult> GetDamageByEachTarget(int combatId)
    {
        var damageByEachTarget = await _damageFilterService.GetDamageByEachTargetAsync(combatId);

        return Ok(damageByEachTarget);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize)
    {
        var damageDones = await _filterService.GetByTargetAsync(combatPlayerId, target, page, pageSize);

        return Ok(damageDones);
    }

    [HttpGet("getValueByTarget")]
    public async Task<IActionResult> GetValueByTarget(int combatPlayerId, string target)
    {
        var valueByTarget = await _filterService.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target);

        return Ok(valueByTarget);
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
        var damageDones = await _filterService.GetBySpellAsync(combatPlayerId, spell, page, pageSize);

        return Ok(damageDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DamageDoneModel damageDone)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid DamageDone create received: {@DamageDone}", damageDone);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<DamageDoneDto>(damageDone);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat damage done.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
