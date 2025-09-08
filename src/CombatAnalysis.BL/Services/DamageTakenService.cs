using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenService(IGenericRepository<DamageTaken> repository, IMapper mapper) : QueryService<DamageTakenDto, DamageTaken>(repository, mapper), IMutationService<DamageTakenDto>
{
    private readonly IGenericRepository<DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<DamageTakenDto> CreateAsync(DamageTakenDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(DamageTakenDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageTakenDto> CreateInternalAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(DamageTakenDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Creator),
                $"The property {nameof(DamageTakenDto.Creator)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Target),
                $"The property {nameof(DamageTakenDto.Target)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Spell),
                $"The property {nameof(DamageTakenDto.Spell)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
    }
}
