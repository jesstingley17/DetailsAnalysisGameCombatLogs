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

    public async Task<PlayerParseInfoDto> CreateAsync(PlayerParseInfoDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerParseInfo>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerParseInfoDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(PlayerParseInfoDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerParseInfo>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(PlayerParseInfoDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecId, nameof(item.SpecId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ClassId, nameof(item.ClassId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.BossId, nameof(item.BossId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Difficult, nameof(item.Difficult));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageEfficiency, nameof(item.DamageEfficiency));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealEfficiency, nameof(item.HealEfficiency));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
