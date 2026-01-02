using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerDeathController(IMutationService<PlayerDeathDto> mutationService, IPlayerInfoService<PlayerDeathDto> service,
    IMapper mapper, ILogger<PlayerDeathController> logger) : ControllerBase
{
    private readonly IMutationService<PlayerDeathDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<PlayerDeathDto> _playerInfoService = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlayerDeathController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> Find(int combatPlayerId)
    {
        var damageTakens = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakens);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlayerDeathModel playerDeath)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PlayerDeath create received: {@PlayerDeath}", playerDeath);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<PlayerDeathDto>(playerDeath);
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
