using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageTakenGeneralController(IMutationServiceBatch<DamageTakenGeneralDto> mutationService,
    IPlayerInfoService<DamageTakenGeneralDto> playerInfoService, IMapper mapper,
    ILogger<DamageTakenGeneralController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<DamageTakenGeneralDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<DamageTakenGeneralDto> _playerInfoService = playerInfoService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DamageTakenGeneralController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var damageTakenGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageTakenGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DamageTakenGeneralModel damageTakenGeneral)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid DamageTakenGeneral create received: {@DamageTakenGeneral}", damageTakenGeneral);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<DamageTakenGeneralDto>(damageTakenGeneral);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat damage taken general.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
