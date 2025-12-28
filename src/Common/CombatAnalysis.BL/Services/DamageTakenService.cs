using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenService(IGenericRepositoryBatch<DamageTaken> repository, IMapper mapper) : IMutationServiceBatch<DamageTakenDto>
{
    private readonly IGenericRepositoryBatch<DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageTakenDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageTaken>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<DamageTakenDto> CreateAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(DamageTakenDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));
        ArgumentException.ThrowIfNullOrEmpty(item.Creator, nameof(item.Creator));
        ArgumentException.ThrowIfNullOrEmpty(item.Target, nameof(item.Target));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ActualValue, nameof(item.ActualValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Resisted, nameof(item.Resisted));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Absorbed, nameof(item.Absorbed));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Blocked, nameof(item.Blocked));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Mitigated, nameof(item.Mitigated));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTakenType, nameof(item.DamageTakenType));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
