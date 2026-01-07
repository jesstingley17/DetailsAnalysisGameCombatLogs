using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerStatsService(IGenericRepository<CombatPlayerStats> repository, IMapper mapper) : QueryService<CombatPlayerStatsDto, CombatPlayerStats>(repository, mapper), IMutationService<CombatPlayerStatsDto>
{
    private readonly IGenericRepository<CombatPlayerStats> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerStatsDto> CreateAsync(CombatPlayerStatsDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerStats>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerStatsDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatPlayerStatsDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayerStats>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatPlayerStatsDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.Strength, nameof(item.Strength));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Agility, nameof(item.Agility));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Intelligence, nameof(item.Intelligence));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Stamina, nameof(item.Stamina));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Spirit, nameof(item.Spirit));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Dodge, nameof(item.Dodge));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Parry, nameof(item.Parry));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Crit, nameof(item.Crit));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Haste, nameof(item.Haste));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Hit, nameof(item.Hit));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Expertise, nameof(item.Expertise));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Armor, nameof(item.Armor));

        ArgumentNullException.ThrowIfNullOrEmpty(item.Talents, nameof(item.Talents));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatPlayerId, 1, nameof(item.CombatPlayerId));
    }
}
