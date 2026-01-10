using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneController(IPlayerInfoPaginationService<DamageDoneDto> playerInfoService, ICountService<DamageDoneDto> countService, IGeneralFilterService<DamageDoneDto> filterService, 
    IDamageFilterService damageFilterService) : ControllerBase
{
    private readonly IPlayerInfoPaginationService<DamageDoneDto> _playerInfoService = playerInfoService;
    private readonly ICountService<DamageDoneDto> _countService = countService;
    private readonly IGeneralFilterService<DamageDoneDto> _filterService = filterService;
    private readonly IDamageFilterService _damageFilterService = damageFilterService;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageDones = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, cancellationToken);

        return Ok(damageDones);
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

    [HttpGet("getDamageByEachTarget/{combatId}")]
    public async Task<IActionResult> GetDamageByEachTarget(int combatId, CancellationToken cancellationToken)
    {
        var damageByEachTarget = await _damageFilterService.GetDamageByEachTargetAsync(combatId, cancellationToken);

        return Ok(damageByEachTarget);
    }

    [HttpGet("getByTarget")]
    public async Task<IActionResult> GetByTarget(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var damageDones = await _filterService.GetByTargetAsync(combatPlayerId, target, page, pageSize, cancellationToken);

        return Ok(damageDones);
    }

    [HttpGet("getValueByTarget")]
    public async Task<IActionResult> GetValueByTarget(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var valueByTarget = await _filterService.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target, cancellationToken);

        return Ok(valueByTarget);
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
        var damageDones = await _filterService.GetBySpellAsync(combatPlayerId, spell, page, pageSize, cancellationToken);

        return Ok(damageDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell, cancellationToken);

        return Ok(count);
    }
}
