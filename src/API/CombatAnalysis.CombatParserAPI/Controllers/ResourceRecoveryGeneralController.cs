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
public class ResourceRecoveryGeneralController(IMutationService<ResourceRecoveryGeneralDto> mutationService, IPlayerInfoService<ResourceRecoveryGeneralDto> playerInfoService,
    IMapper mapper, ILogger<ResourceRecoveryGeneralController> logger) : ControllerBase
{
    private readonly IMutationService<ResourceRecoveryGeneralDto> _mutationService = mutationService;
    private readonly IPlayerInfoService<ResourceRecoveryGeneralDto> _playerInfoService = playerInfoService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ResourceRecoveryGeneralController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var resourceRecoveryGenerals = await _playerInfoService.GetByCombatPlayerIdAsync(combatPlayerId);

        return Ok(resourceRecoveryGenerals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ResourceRecoveryGeneralModel resourceRecoveryGeneral)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid ResourceRecoveryGeneral create received: {@ResourceRecoveryGeneral}", resourceRecoveryGeneral);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<ResourceRecoveryGeneralDto>(resourceRecoveryGeneral);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create resource recovery general.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
