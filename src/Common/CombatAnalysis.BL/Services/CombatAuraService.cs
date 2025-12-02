using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatAuraService(IGenericRepository<CombatAura> repository, IMapper mapper) : QueryService<CombatAuraDto, CombatAura>(repository, mapper), IMutationService<CombatAuraDto>
{
    private readonly IGenericRepository<CombatAura> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatAuraDto> CreateAsync(CombatAuraDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatAura>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatAuraDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatAuraDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatAura>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatAuraDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Name, nameof(item.Name));
        ArgumentException.ThrowIfNullOrEmpty(item.Creator, nameof(item.Creator));
        ArgumentException.ThrowIfNullOrEmpty(item.Target, nameof(item.Target));

        ArgumentOutOfRangeException.ThrowIfNegative(item.AuraCreatorType, nameof(item.AuraCreatorType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.AuraType, nameof(item.AuraType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Stacks, nameof(item.Stacks));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatId, 1, nameof(item.CombatId));
    }
}
