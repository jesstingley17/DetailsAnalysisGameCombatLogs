using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService(IGenericRepositoryBatch<DamageTakenGeneral> repository, IMapper mapper) : IMutationServiceBatch<DamageTakenGeneralDto>
{
    private readonly IGenericRepositoryBatch<DamageTakenGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageTakenGeneralDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageTakenGeneral>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<DamageTakenGeneralDto> CreateAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(DamageTakenGeneralDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTakenPerSecond, nameof(item.DamageTakenPerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CritNumber, nameof(item.CritNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MissNumber, nameof(item.MissNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CastNumber, nameof(item.CastNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MinValue, nameof(item.MinValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MaxValue, nameof(item.MaxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageValue, nameof(item.AverageValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
