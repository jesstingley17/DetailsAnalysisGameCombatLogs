using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneGeneralService(IGenericRepository<HealDoneGeneral> repository, IMapper mapper) : QueryService<HealDoneGeneralDto, HealDoneGeneral>(repository, mapper), IMutationService<HealDoneGeneralDto>
{
    private readonly IGenericRepository<HealDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<HealDoneGeneralDto> CreateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<HealDoneGeneralDto> CreateInternalAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(HealDoneGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto.Spell),
                $"The property {nameof(HealDoneGeneralDto.Spell)} of the {nameof(HealDoneGeneralDto)} object can't be null or empty");
        }
    }
}
