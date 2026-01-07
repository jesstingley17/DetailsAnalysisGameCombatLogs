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
public class DamageTakenController(IMutationServiceBatch<DamageTakenDto> mutationService, IPlayerInfoPaginationService<DamageTakenDto> playerInfoService,
    ICountService<DamageTakenDto> countService, IGeneralFilterService<DamageTakenDto> filterService,
    IMapper mapper, ILogger<DamageTakenController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<DamageTakenDto> _mutationService = mutationService;
    private readonly IPlayerInfoPaginationService<DamageTakenDto> _playerInfoService = playerInfoService;
    private readonly ICountService<DamageTakenDto> _countService = countService;
    private readonly IGeneralFilterService<DamageTakenDto> _filterService = filterService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DamageTakenController> _logger = logger;

    [HttpGet("getByCombatPlayerId")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId, int page, int pageSize)
    {
        var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        return Ok(damageTakens);
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
        var damageTakens = await _filterService.GetByCreatorAsync(combatPlayerId, creator, page, pageSize);

        return Ok(damageTakens);
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
        var daamgeTakens = await _filterService.GetBySpellAsync(combatPlayerId, spell, page, pageSize);

        return Ok(daamgeTakens);
    }

    [HttpGet("countBySpell")]
    public async Task<IActionResult> CountBySpell(int combatPlayerId, string spell)
    {
        var count = await _filterService.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        return Ok(count);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DamageTakenModel damageTaken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid DamageTaken create received: {@DamageTaken}", damageTaken);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<DamageTakenDto>(damageTaken);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat damage taken.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
