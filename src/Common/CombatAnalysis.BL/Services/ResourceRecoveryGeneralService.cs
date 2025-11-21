using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : IMutationService<ResourceRecoveryGeneralDto>
{
    private readonly IGenericRepository<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceRecoveryGeneralDto> CreateAsync(ResourceRecoveryGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(ResourceRecoveryGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(ResourceRecoveryGeneralDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));

        ArgumentOutOfRangeException.ThrowIfNegative(item.Value, nameof(item.Value));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ResourcePerSecond, nameof(item.ResourcePerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CastNumber, nameof(item.CastNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MinValue, nameof(item.MinValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MaxValue, nameof(item.MaxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageValue, nameof(item.AverageValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
