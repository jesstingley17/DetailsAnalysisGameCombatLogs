using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneService(IGenericRepository<DamageDone> repository, IMapper mapper) : QueryService<DamageDoneDto, DamageDone>(repository, mapper), IMutationService<DamageDoneDto>
{
    private readonly IGenericRepository<DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<DamageDoneDto> CreateAsync(DamageDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(DamageDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageDoneDto> CreateInternalAsync(DamageDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDone>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageDoneDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDone>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(DamageDoneDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Creator),
                $"The property {nameof(DamageDoneDto.Creator)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Target),
                $"The property {nameof(DamageDoneDto.Target)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Spell),
                $"The property {nameof(DamageDoneDto.Spell)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
    }
}
