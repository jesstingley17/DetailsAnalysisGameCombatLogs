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
    IMutationService<CombatPlayerDto> mutationCombatPlayerService, IMutationService<PlayerStatsDto> mutationPlayerStatsService, IMapper mapper,
    ILogger<CombatController> logger, ICombatDataHelper saveCombatDataHelper,
    ICombatTransactionService combatTransactionService) : ControllerBase
{
    private readonly IBossService _bossService = bossService;
    private readonly IQueryService<CombatDto> _queryCombatService = queryCombatService;
    private readonly IMutationService<CombatDto> _mutationCombatService = mutationCombatService;
    private readonly IMutationService<CombatPlayerDto> _mutationCombatPlayerService = mutationCombatPlayerService;
    private readonly IMutationService<PlayerStatsDto> _mutationPlayerStatsService = mutationPlayerStatsService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatController> _logger = logger;
    private readonly ICombatDataHelper _saveCombatDataHelper = saveCombatDataHelper;
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

            // Transaction to create Combat and Combat Players data: combat players, damage, heal, damage taken, resources, etc
            await _combatTransactionService.BeginTransactionAsync();

            var createdCombat = await CreateCombatAsync(combat);
            combat.Id = createdCombat.Id;

            var combatDetails = await CreateCombatPlayersAsync(combat);

            await _combatTransactionService.CommitTransactionAsync();

            // Transaction to create Combat player specialization score: how combat player do mechanics, class rotation, assist other combat players, etc
            await _combatTransactionService.BeginTransactionAsync();

            await CreateCombatPlayersSpecializationScoreAsync(combat, combatDetails);

            await _combatTransactionService.CommitTransactionAsync();

            await UpdateCombatAsync(createdCombat);

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
    
    private async Task UpdateCombatAsync(CombatDto combat)
    {
        combat.IsReady = true;
        await _mutationCombatService.UpdateAsync(combat);
    }

    private async Task<CombatDetails> CreateCombatPlayersAsync(CombatModel combat)
    {
        foreach (var player in combat.CombatPlayers) 
        {
            player.CombatId = combat.Id;

            var createdCombatPlayer = await CreateCombatPlayerAsync(player);
            player.Id = createdCombatPlayer.Id;

            player.Stats.CombatPlayerId = player.Id;

            await CreatePlayerStatsAsync(player);
        }

        var combatDetails = await _saveCombatDataHelper.CreateCombatPlayersDataAsync(combat);

        await UpdateCombatPlayersAsync(combat.CombatPlayers);

        return combatDetails;
    }

    private async Task CreateCombatPlayersSpecializationScoreAsync(CombatModel combat, CombatDetails combatDetails)
    {
        await _saveCombatDataHelper.CreateSpecializationScoreAsync(combat.CombatPlayers, combatDetails, combat.Boss.Id);
        await UpdateCombatPlayersAsync(combat.CombatPlayers);
    }

    private async Task<CombatPlayerDto> CreateCombatPlayerAsync(CombatPlayerModel model)
    {
        var map = _mapper.Map<CombatPlayerDto>(model);
        var createdCombatPlayer = await _mutationCombatPlayerService.CreateAsync(map);
        ArgumentNullException.ThrowIfNull(createdCombatPlayer, nameof(createdCombatPlayer));

        return createdCombatPlayer;
    }

    private async Task CreatePlayerStatsAsync(CombatPlayerModel player)
    {
        var map = _mapper.Map<PlayerStatsDto>(player.Stats);
        var createdStats = await _mutationPlayerStatsService.CreateAsync(map);

        ArgumentNullException.ThrowIfNull(createdStats, nameof(createdStats));
    }

    private async Task UpdateCombatPlayersAsync(List<CombatPlayerModel> combatPlayers)
    {
        foreach (var combatPlayer in combatPlayers)
        {
            var map = _mapper.Map<CombatPlayerDto>(combatPlayer);
            await _mutationCombatPlayerService.UpdateAsync(map);
        }
    }
}
