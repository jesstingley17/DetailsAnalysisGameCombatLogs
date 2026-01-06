using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatPlayerController(ICombatPlayerService service, IMapper mapper, ILogger<CombatPlayerController> logger) : ControllerBase
{
    private readonly ICombatPlayerService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatPlayerController> _logger = logger;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var combatPlayers = await _service.GetByCombatIdAsync(combatId);

        return Ok(combatPlayers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatPlayerModel combatPlayer)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CombatPlayer create received: {@CombatPlayer}", combatPlayer);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CombatPlayerDto>(combatPlayer);
            var createdItem = await _service.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat player.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
