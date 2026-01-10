using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResourceRecoveryController(IPlayerInfoPaginationService<ResourceRecoveryDto> playerInfoService, ICountService<ResourceRecoveryDto> countService, IGeneralFilterService<ResourceRecoveryDto> filterService) : ControllerBase
{
    private readonly IPlayerInfoPaginationService<ResourceRecoveryDto> _playerInfoService = playerInfoService;
    private readonly ICountService<ResourceRecoveryDto> _countService = countService;
    private readonly IGeneralFilterService<ResourceRecoveryDto> _filterService = filterService;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resourcesRecoveries = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, cancellationToken);

        return Ok(resourcesRecoveries);
    }
    
    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _countService.CountByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(count);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _filterService.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return Ok(uniqueTargets);
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        var resourceRecoveries = await _filterService.GetByCreatorAsync(combatPlayerId, creator, page, pageSize, cancellationToken);

        return Ok(resourceRecoveries);
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        var count = await _filterService.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator, cancellationToken);

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
