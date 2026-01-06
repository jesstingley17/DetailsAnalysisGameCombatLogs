using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerStatsController(IQueryService<CombatPlayerStatsDto> queryService, IMutationService<CombatPlayerStatsDto> mutationService,
    IMapper mapper, ILogger<PlayerStatsController> logger) : ControllerBase
{
    private readonly IQueryService<CombatPlayerStatsDto> _queryService = queryService;
    private readonly IMutationService<CombatPlayerStatsDto> _mutationService = mutationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlayerStatsController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var playerStats = await _queryService.GetByParamAsync(nameof(CombatPlayerStatsModel.CombatPlayerId), combatPlayerId);
        var firstPlayerStats = playerStats.SingleOrDefault();

        return Ok(firstPlayerStats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var playerParseInfo = await _queryService.GetByIdAsync(id);

        return Ok(playerParseInfo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatPlayerStatsModel playerStats)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PlayerStats create received: {@PlayerStats}", playerStats);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatPlayerStatsDto>(playerStats);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create player stats.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
