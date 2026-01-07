using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController(IMutationServiceBatch<CombatPlayerDeathDto> mutationService, IPlayerInfoPaginationService<CombatPlayerDeathDto> service,
    IMapper mapper, ILogger<PlayerDeathController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<CombatPlayerDeathDto> _mutationService = mutationService;
    private readonly IPlayerInfoPaginationService<CombatPlayerDeathDto> _playerInfoService = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlayerDeathController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakens);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatPlayerDeathModel playerDeath)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PlayerDeath create received: {@PlayerDeath}", playerDeath);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatPlayerDeathDto>(playerDeath);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create player death.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
