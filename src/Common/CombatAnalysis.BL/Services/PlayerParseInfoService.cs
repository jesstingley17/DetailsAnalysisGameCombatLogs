using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerParseInfoService(IGenericRepository<PlayerParseInfo> repository, IMapper mapper) : QueryService<PlayerParseInfoDto, PlayerParseInfo>(repository, mapper), IMutationService<PlayerParseInfoDto>
{
    private readonly IGenericRepository<PlayerParseInfo> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public Task<PlayerParseInfoDto> CreateAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<PlayerParseInfoDto> CreateInternalAsync(PlayerParseInfoDto item)
    {
        var map = _mapper.Map<PlayerParseInfo>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerParseInfoDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PlayerParseInfoDto item)
    {
        var map = _mapper.Map<PlayerParseInfo>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private void CheckParams(PlayerParseInfoDto item)
    {
        if (item.Difficult < 0)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto.Difficult),
                $"The property {nameof(PlayerParseInfoDto.Difficult)} of the {nameof(PlayerParseInfoDto)} should be positive");
        }
    }
}
