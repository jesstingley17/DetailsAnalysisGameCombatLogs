using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerPositionService(IGenericRepository<CombatPlayerPosition> repository, IMapper mapper) : QueryService<CombatPlayerPositionDto, CombatPlayerPosition>(repository, mapper), IMutationService<CombatPlayerPositionDto>
{
    private readonly IGenericRepository<CombatPlayerPosition> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerPositionDto> CreateAsync(CombatPlayerPositionDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerPosition>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerPositionDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatPlayerPositionDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerPosition>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatPlayerPositionDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatId, 1, nameof(item.CombatId));
        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
