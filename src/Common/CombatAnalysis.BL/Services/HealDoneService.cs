using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneService(IGenericRepositoryBatch<HealDone> repository, IMapper mapper) : IMutationServiceBatch<HealDoneDto>
{
    private readonly IGenericRepositoryBatch<HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<HealDoneDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<HealDone>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<HealDoneDto> CreateAsync(HealDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDone>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(HealDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDone>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(HealDoneDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));
        ArgumentException.ThrowIfNullOrEmpty(item.Creator, nameof(item.Creator));
        ArgumentException.ThrowIfNullOrEmpty(item.Target, nameof(item.Target));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Overheal, nameof(item.Overheal));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
