using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatService(ICreateEntityRepository<Combat> repository, IMapper mapper) : QueryService<CombatDto, Combat>(repository, mapper), IMutationService<CombatDto>
{
    private readonly ICreateEntityRepository<Combat> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatDto> CreateAsync(CombatDto item, CancellationToken cancelationToken)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var createdItem = await _repository.CreateAsync(map, cancelationToken);
        var resultMap = _mapper.Map<CombatDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatDto item, CancellationToken cancelationToken)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var affectedRows = await _repository.UpdateAsync(map, cancelationToken);

        return affectedRows;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancelationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id, cancelationToken);

        return entityDeleted;
    }

    private static void CheckParams(CombatDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.DungeonName, nameof(item.DungeonName));

        ArgumentOutOfRangeException.ThrowIfNegative(item.ResourcesRecovery, nameof(item.ResourcesRecovery));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTaken, nameof(item.DamageTaken));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatLogId, 1, nameof(item.CombatLogId));
    }
}
