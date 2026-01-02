using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DamageDoneGeneralController(IMutationServiceBatch<DamageDoneGeneralDto> mutationService, IPlayerInfoService<DamageDoneGeneralDto> playerInfoService,
    IMapper mapper, ILogger<DamageDoneGeneralController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<DamageDoneGeneralDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<DamageDoneGeneralDto> _playerInfoService = playerInfoService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DamageDoneGeneralController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var damageDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(damageDoneGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DamageDoneGeneralModel damageDoneGeneral)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid DamageDoneGeneral create received: {@DamageDoneGeneral}", damageDoneGeneral);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<DamageDoneGeneralDto>(damageDoneGeneral);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat damage done general.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
