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
public class ResourceRecoveryController(IMutationService<ResourceRecoveryDto> mutationService, IPlayerInfoService<ResourceRecoveryDto> playerInfoService,
    ICountService<ResourceRecoveryDto> countService, IGeneralFilterService<ResourceRecoveryDto> filterService,
    IMapper mapper, ILogger<ResourceRecoveryController> logger) : ControllerBase
{
    private readonly IMutationService<ResourceRecoveryDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<ResourceRecoveryDto> _playerInfoService = playerInfoService;
    private readonly ICountService<ResourceRecoveryDto> _countService = countService;
    private readonly IGeneralFilterService<ResourceRecoveryDto> _filterService = filterService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ResourceRecoveryController> _logger = logger;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var resourcesRecoveries = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        return Ok(resourcesRecoveries);
    }
    
    [HttpGet("count/{combatPlayerId}")]
    public async Task<IActionResult> Count(int combatPlayerId)
    {
        var count = await _countService.CountByCombatPlayerIdAsync(combatPlayerId);

        return Ok(count);
    }

    [HttpGet("getUniqueCreators/{combatPlayerId}")]
    public async Task<IActionResult> GetUniqueCreators(int combatPlayerId)
    {
        var uniqueTargets = await _filterService.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId);

        return Ok(uniqueTargets);
    }

    [HttpGet("getByCreator")]
    public async Task<IActionResult> GetByCreator(int combatPlayerId, string creator, int page, int pageSize)
    {
        var resourceRecoveries = await _filterService.GetCreatorByCombatPlayerIdAsync(combatPlayerId, creator, page, pageSize);

        return Ok(resourceRecoveries);
    }

    [HttpGet("countByCreator")]
    public async Task<IActionResult> CountByCreator(int combatPlayerId, string creator)
    {
        var count = await _filterService.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

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
        var healDones = await _filterService.GetSpellByCombatPlayerIdAsync(combatPlayerId, spell, page, pageSize);

        return Ok(healDones);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ResourceRecoveryModel resourceRecovery)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ResourceRecovery create received: {@ResourceRecovery}", resourceRecovery);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<ResourceRecoveryDto>(resourceRecovery);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(_mapper);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create resource recovery.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
