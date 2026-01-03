using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerService(IGenericRepository<CombatPlayer> repository, IMapper mapper) : QueryService<CombatPlayerDto, CombatPlayer>(repository, mapper), IMutationService<CombatPlayerDto>
{
    private readonly IGenericRepository<CombatPlayer> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerDto> CreateAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatPlayerDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.PlayerId, nameof(item.PlayerId));

        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageItemLevel, nameof(item.AverageItemLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ResourcesRecovery, nameof(item.ResourcesRecovery));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTaken, nameof(item.DamageTaken));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatId, 1, nameof(item.CombatId));
    }
}
