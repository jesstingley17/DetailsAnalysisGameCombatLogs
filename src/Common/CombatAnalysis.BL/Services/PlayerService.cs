using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class PlayerService(IPlayerRepository repository, IMapper mapper) : IPlayerService
{
    private readonly IPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PlayerDto> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PlayerDto>(result);

        return resultMap;
    }

    public async Task<PlayerDto> GetByGameIdAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));

        var result = await _repository.GetByGameIdAsync(gameId);
        var resultMap = _mapper.Map<PlayerDto>(result);

        return resultMap;
    }

    public async Task<PlayerDto> CreateAsync(PlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Player>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerDto>(createdItem);

        return resultMap;
    }

    private static void CheckParams(PlayerDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Id, nameof(item.Id));
        ArgumentException.ThrowIfNullOrEmpty(item.GameId, nameof(item.GameId));
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Faction, nameof(item.Faction));
    }
}
