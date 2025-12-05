using AutoMapper;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerDeathService(IGenericRepository<PlayerDeath> repository, IMapper mapper) : QueryService<PlayerDeathDto, PlayerDeath>(repository, mapper), IMutationService<PlayerDeathDto>
{
    private readonly IGenericRepository<PlayerDeath> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PlayerDeathDto> CreateAsync(PlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerDeath>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerDeathDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(PlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerDeath>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(PlayerDeathDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));

        ArgumentOutOfRangeException.ThrowIfNegative(item.LastHitValue, nameof(item.LastHitValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
