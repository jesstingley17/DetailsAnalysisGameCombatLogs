using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CombatController(IBossService bossService, IQueryService<CombatDto> queryCombatService, IMutationService<CombatDto> mutationCombatService,
    ICombatPlayerService combatPlayerService, IMapper mapper, ILogger<CombatController> logger, 
    ISpecializationScoreHelper scoreHelper, ICombatDataHelper combatDataHelper, ICombatTransactionService combatTransactionService) : ControllerBase
{
    private readonly IBossService _bossService = bossService;
    private readonly IQueryService<CombatDto> _queryCombatService = queryCombatService;
    private readonly IMutationService<CombatDto> _mutationCombatService = mutationCombatService;
    private readonly ICombatPlayerService _combatPlayerService = combatPlayerService;
    private readonly ISpecializationScoreHelper _scoreHelper = scoreHelper;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatController> _logger = logger;
    private readonly ICombatDataHelper _combatDataHelper = combatDataHelper;
    private readonly ICombatTransactionService _combatTransactionService = combatTransactionService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var combats = await _queryCombatService.GetAllAsync();

        return Ok(combats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var combat = await _queryCombatService.GetByIdAsync(id);

        return Ok(combat);
    }

    [HttpGet("getByCombatLogId/{combatLogId:int:min(1)}")]
    public async Task<IActionResult> GetByCombatLogId(int combatLogId)
    {
        var combats = await _queryCombatService.GetByParamAsync(nameof(CombatModel.CombatLogId), combatLogId);
        var map = _mapper.Map<IEnumerable<CombatModel>>(combats);
        foreach (var item in map)
        {
            var boss = await _bossService.GetById(item.Boss.Id);
            var bossMap = _mapper.Map<BossModel>(boss);

            item.Boss = bossMap;
        }

        return Ok(map);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CombatModel combat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Combat create received: {@Combat}", combat);

                return ValidationProblem(ModelState);
            }

            // A huge transaction for all action as ONE TRANSACTION was divided into a few small transactions.
            // Transactions split by logic: combat/combatPlayer/combatPlayerData and combatPlayerSpecScore/bestSpecScore

            // Transaction to create Combat and Combat Players
            await _combatTransactionService.BeginTransactionAsync();

            var createdCombat = await CreateCombatAsync(combat);
            combat.Id = createdCombat.Id;

            var combatDetails = await CreateCombatPlayersAsync(combat);

            await _combatTransactionService.CommitTransactionAsync();

            // Transaction to create Combat player data and specialization score:
            // 1) damage, heal, damage taken, resources, etc
            // 2) how combat player do mechanics, class rotation, assist other combat players, etc
            await _combatTransactionService.BeginTransactionAsync();

            var createdCombatPlayers = await _combatPlayerService.GetByCombatIdAsync(combat.Id);
            await _combatDataHelper.CreateCombatPlayersDataAsync(combatDetails, [.. createdCombatPlayers], combat.Id);
            await _combatDataHelper.UpdateSpecializationScoreAsync([.. createdCombatPlayers], combatDetails, combat.Boss.Id);

            createdCombat.IsReady = true;

            await _mutationCombatService.UpdateAsync(createdCombat);

            await _combatTransactionService.CommitTransactionAsync();

            return Ok(createdCombat);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create combat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CombatModel combat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Combat update request received: {@Combat}", combat);

                return ValidationProblem(ModelState);
            }

            if (id != combat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<CombatDto>(combat);
            await _mutationCombatService.UpdateAsync(map);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var entityDeleted = await _mutationCombatService.DeleteAsync(id);
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

    private async Task<CombatDto> CreateCombatAsync(CombatModel model)
    {
        var map = _mapper.Map<CombatDto>(model);
        var createdCombat = await _mutationCombatService.CreateAsync(map);
        ArgumentNullException.ThrowIfNull(createdCombat, nameof(createdCombat));

        return createdCombat;
    }

    private async Task<CombatDetails> CreateCombatPlayersAsync(CombatModel combat)
    {
        var combatDetails = _combatDataHelper.CreateCombatDetails(combat);

        combat.CombatPlayers = [.. combat.CombatPlayers.Select(cp =>
        {
            cp.CombatId = combat.Id;

            return cp;
        })];

        var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(combat.CombatPlayers);
        foreach (var combatPlayer in map)
        {
            await _scoreHelper.CreateSpecializationScoreAsync(combatPlayer, combatDetails);
        }
        
        await _combatPlayerService.CreateBatchAsync(map);

        return combatDetails;
    }
}
