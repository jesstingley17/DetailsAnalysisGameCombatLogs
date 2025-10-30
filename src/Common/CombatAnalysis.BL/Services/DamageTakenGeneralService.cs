using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService(IGenericRepository<DamageTakenGeneral> repository, IMapper mapper) : QueryService<DamageTakenGeneralDto, DamageTakenGeneral>(repository, mapper), IMutationService<DamageTakenGeneralDto>
{
    private readonly IGenericRepository<DamageTakenGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<DamageTakenGeneralDto> CreateAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageTakenGeneralDto> CreateInternalAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto.Spell),
                $"The property {nameof(DamageTakenGeneralDto.Spell)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }
    }
}
