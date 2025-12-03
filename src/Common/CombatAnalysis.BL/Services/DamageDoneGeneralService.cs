using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneGeneralService(IGenericRepository<DamageDoneGeneral> repository, IMapper mapper) : IMutationService<DamageDoneGeneralDto>
{
    private readonly IGenericRepository<DamageDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DamageDoneGeneralDto> CreateAsync(DamageDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageDoneGeneralDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(DamageDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(DamageDoneGeneralDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamagePerSecond, nameof(item.DamagePerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CritNumber, nameof(item.CritNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MissNumber, nameof(item.MissNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CastNumber, nameof(item.CastNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MinValue, nameof(item.MinValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MaxValue, nameof(item.MaxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageValue, nameof(item.AverageValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
