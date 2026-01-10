using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneController(IPlayerInfoPaginationService<HealDoneDto> playerInfoService, ICountService<HealDoneDto> countService, IGeneralFilterService<HealDoneDto> filterService) : ControllerBase
{
    private readonly IPlayerInfoPaginationService<HealDoneDto> _playerInfoService = playerInfoService;
    private readonly ICountService<HealDoneDto> _countService = countService;
    private readonly IGeneralFilterService<HealDoneDto> _filterService = filterService;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var healDones = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, cancellationToken);

        return Ok(healDones);
    }

    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _countService.CountByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueTargets/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueTargets(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _filterService.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var healDones = await _filterService.GetByTargetAsync(combatPlayerId, target, page, pageSize, cancellationToken);

        return Ok(healDones);
    }

    [HttpGet("countByTarget")]
    public async Task<IActionResult> CountByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _filterService.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target, cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueSpells/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueSpells(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _filterService.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(uniqueSpells);
    }

    [HttpGet("getBySpell")]
    public async Task<IActionResult> GetBySpell(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var healDones = await _filterService.GetBySpellAsync(combatPlayerId, spell, page, pageSize, cancellationToken);

        return Ok(healDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell, cancellationToken);

        return Ok(count);
    }
}
