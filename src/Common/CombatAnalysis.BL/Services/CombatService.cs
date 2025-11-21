using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatService(IGenericRepository<Combat> repository, IMapper mapper) : QueryService<CombatDto, Combat>(repository, mapper), IMutationService<CombatDto>
{
    private readonly IGenericRepository<Combat> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatDto> CreateAsync(CombatDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var affectedRows = await _repository.UpdateAsync(map);

        return affectedRows;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Name, nameof(item.Name));
        ArgumentException.ThrowIfNullOrEmpty(item.DungeonName, nameof(item.DungeonName));
        ArgumentException.ThrowIfNullOrEmpty(item.Name, nameof(item.Name));

        ArgumentOutOfRangeException.ThrowIfNegative(item.LocallyNumber, nameof(item.LocallyNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Difficulty, nameof(item.Difficulty));
        ArgumentOutOfRangeException.ThrowIfNegative(item.EnergyRecovery, nameof(item.EnergyRecovery));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTaken, nameof(item.DamageTaken));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatLogId, 1, nameof(item.CombatLogId));
    }
}
