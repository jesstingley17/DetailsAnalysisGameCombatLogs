using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerDeathService(IGenericRepositoryBatch<CombatPlayerDeath> repository, IMapper mapper) : QueryService<CombatPlayerDeathDto, CombatPlayerDeath>(repository, mapper), IMutationServiceBatch<CombatPlayerDeathDto>
{
    private readonly IGenericRepositoryBatch<CombatPlayerDeath> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<CombatPlayerDeathDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<CombatPlayerDeath>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<CombatPlayerDeathDto> CreateAsync(CombatPlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerDeath>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerDeathDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatPlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerDeath>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatPlayerDeathDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));

        ArgumentOutOfRangeException.ThrowIfNegative(item.LastHitValue, nameof(item.LastHitValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
