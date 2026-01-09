using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerController(IPlayerService service, IMapper mapper, ILogger<PlayerController> logger) : ControllerBase
{
    private readonly IPlayerService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlayerController> _logger = logger;

    [HttpGet("getByGamePlayerId/{gamePlayerId}")]
    public async Task<IActionResult> GetByGamePlayerId(string gamePlayerId, CancellationToken cancellationToken)
    {
        var player = await _service.GetByGameIdAsync(gamePlayerId, cancellationToken);

        return Ok(player);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlayerModel player, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Player create received: {@Player}", player);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<PlayerDto>(player);
            var createdItem = await _service.CreateAsync(map, cancellationToken);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create player.");

            return StatusCode(500, "Internal server error.");
        }
    }
}