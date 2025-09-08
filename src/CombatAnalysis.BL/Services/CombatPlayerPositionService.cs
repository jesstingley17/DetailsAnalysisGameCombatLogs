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

    public Task<CombatPlayerPositionDto> CreateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CombatPlayerPositionDto> CreateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerPositionDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
