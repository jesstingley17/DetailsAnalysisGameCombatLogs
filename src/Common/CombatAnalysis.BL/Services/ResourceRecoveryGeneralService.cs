using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService(IGenericRepositoryBatch<ResourceRecoveryGeneral> repository, IMapper mapper) : IMutationServiceBatch<ResourceRecoveryGeneralDto>
{
    private readonly IGenericRepositoryBatch<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<ResourceRecoveryGeneralDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneral>>(items);
        await _repository.CreateBatchAsync(map);
    }

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
        ArgumentException.ThrowIfNullOrEmpty(item.Spell, nameof(item.Spell));

        ArgumentOutOfRangeException.ThrowIfNegative(item.CastNumber, nameof(item.CastNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(item.MaxValue, nameof(item.MaxValue));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
