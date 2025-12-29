using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerParseInfoService(IGenericRepositoryBatch<PlayerParseInfo> repository, IMapper mapper) : QueryService<PlayerParseInfoDto, PlayerParseInfo>(repository, mapper), IMutationServiceBatch<PlayerParseInfoDto>
{
    private readonly IGenericRepositoryBatch<PlayerParseInfo> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<PlayerParseInfoDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<PlayerParseInfo>>(items);
        await _repository.CreateBatchAsync(map);
    }

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
        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecId, nameof(item.SpecId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ClassId, nameof(item.ClassId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.BossId, nameof(item.BossId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Difficult, nameof(item.Difficult));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageEfficiency, nameof(item.DamageEfficiency));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealEfficiency, nameof(item.HealEfficiency));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
