using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryService(IGenericRepository<ResourceRecovery> repository, IMapper mapper) : QueryService<ResourceRecoveryDto, ResourceRecovery>(repository, mapper), IMutationService<ResourceRecoveryDto>
{
    private readonly IGenericRepository<ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceRecoveryDto> CreateAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<ResourceRecoveryDto> CreateInternalAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Creator),
                $"The property {nameof(ResourceRecoveryDto.Creator)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Target),
                $"The property {nameof(ResourceRecoveryDto.Target)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Spell),
                $"The property {nameof(ResourceRecoveryDto.Spell)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
    }
}
