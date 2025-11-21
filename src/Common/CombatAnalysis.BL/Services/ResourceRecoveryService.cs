using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryService(IGenericRepository<ResourceRecovery> repository, IMapper mapper) : IMutationService<ResourceRecoveryDto>
{
    private readonly IGenericRepository<ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceRecoveryDto> CreateAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(ResourceRecoveryDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));
        ArgumentException.ThrowIfNullOrEmpty(item.Creator, nameof(item.Creator));
        ArgumentException.ThrowIfNullOrEmpty(item.Target, nameof(item.Target));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
