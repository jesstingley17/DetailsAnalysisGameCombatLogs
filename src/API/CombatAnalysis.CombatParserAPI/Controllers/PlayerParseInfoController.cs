using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerParseInfoController(IQueryService<PlayerParseInfoDto> queryService, IMutationService<PlayerParseInfoDto> mutationService,
    IMapper mapper, ILogger<PlayerParseInfoController> logger) : ControllerBase
{
    private readonly IQueryService<PlayerParseInfoDto> _queryService = queryService;
    private readonly IMutationService<PlayerParseInfoDto> _mutationService = mutationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlayerParseInfoController> _logger = logger;

    [HttpGet("getByCombatPlayerId/{combatPlayerId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatPlayerId(int combatPlayerId)
    {
        var playerParseInfo = await _queryService.GetByParamAsync(nameof(PlayerParseInfoModel.CombatPlayerId), combatPlayerId);

        return Ok(playerParseInfo);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var playerParseInfo = await _queryService.GetByIdAsync(id);

        return Ok(playerParseInfo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlayerParseInfoModel playerParseInfo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PlayerParseInfo create received: {@PlayerParseInfo}", playerParseInfo);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<PlayerParseInfoDto>(playerParseInfo);
            var createdItem = await _mutationService.CreateAsync(map);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create player parse info.");

            return StatusCode(500, "Internal server error.");
        }
    }
}
