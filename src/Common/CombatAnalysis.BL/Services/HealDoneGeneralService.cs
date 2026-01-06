using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneGeneralService(IGenericRepositoryBatch<HealDoneGeneral> repository, IMapper mapper) : IMutationServiceBatch<HealDoneGeneralDto>
{
    private readonly IGenericRepositoryBatch<HealDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<HealDoneGeneralDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<HealDoneGeneral>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<HealDoneGeneralDto> CreateAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneGeneralDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(HealDoneGeneralDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealPerSecond, nameof(item.HealPerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CritNumber, nameof(item.CritNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CastNumber, nameof(item.CastNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MinValue, nameof(item.MinValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MaxValue, nameof(item.MaxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageValue, nameof(item.AverageValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
