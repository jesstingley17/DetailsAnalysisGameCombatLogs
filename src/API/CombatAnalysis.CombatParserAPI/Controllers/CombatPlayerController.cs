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
public class CombatPlayerController(IQueryService<CombatPlayerDto> queryCombatPlayerService, IMutationService<CombatPlayerDto> mutationCombatPlayerService, IPlayerService _playerService,
    IMapper mapper, ILogger<CombatPlayerController> logger) : ControllerBase
{
    private readonly IQueryService<CombatPlayerDto> _queryCombatPlayerService = queryCombatPlayerService;
    private readonly IMutationService<CombatPlayerDto> _mutationCombatPlayerService = mutationCombatPlayerService;
    private readonly IPlayerService _playerService = _playerService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatPlayerController> _logger = logger;

    [HttpGet("getByCombatId/{combatId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatId(int combatId)
    {
        var combatPlayers = await _queryCombatPlayerService.GetByParamAsync(nameof(CombatPlayerModel.CombatId), combatId);
        var map = _mapper.Map<IEnumerable<CombatPlayerModel>>(combatPlayers);
        foreach (var item in map)
        {
            var player = await _playerService.GetByIdAsync(item.Player.Id);
            var playerMap = _mapper.Map<PlayerModel>(player);

            item.Player = playerMap;
        }

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combatPlayer = await _queryCombatPlayerService.GetByIdAsync(id);
        var map = _mapper.Map<CombatPlayerModel>(combatPlayer);
        var player = await _playerService.GetByIdAsync(map.Player.Id);
        var playerMap = _mapper.Map<PlayerModel>(player);

        map.Player = playerMap;

        return Ok(map);
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
            var createdItem = await _mutationCombatPlayerService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat player.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var entityDeleted = await _mutationCombatPlayerService.DeleteAsync(id);
            if (!entityDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
