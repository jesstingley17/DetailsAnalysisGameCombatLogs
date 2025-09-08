using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : QueryService<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>(repository, mapper), IMutationService<ResourceRecoveryGeneralDto>
{
    private readonly IGenericRepository<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<ResourceRecoveryGeneralDto> CreateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<ResourceRecoveryGeneralDto> CreateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto.Spell),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }
    }
}
