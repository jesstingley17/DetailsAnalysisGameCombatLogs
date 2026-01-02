using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class HealDoneGeneralController(IMutationServiceBatch<HealDoneGeneralDto> mutationService, IPlayerInfoService<HealDoneGeneralDto> playerInfoService,
    IMapper mapper, ILogger<CombatPlayerController> logger) : ControllerBase
{
    private readonly IMutationServiceBatch<HealDoneGeneralDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<HealDoneGeneralDto> _playerInfoService = playerInfoService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatPlayerController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var healDoneGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(healDoneGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HealDoneGeneralModel healDoneGeneral)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid HealDoneGeneral create received: {@HealDoneGeneral}", healDoneGeneral);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<HealDoneGeneralDto>(healDoneGeneral);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create heal done general.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
