using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerService(ICombatPlayerRepository repository, IMapper mapper) : ICombatPlayerService
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerDto> CreateAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(int id, CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var rowsAffected = await _repository.UpdateAsync(id, map);

        return rowsAffected;
    }

    public async Task<IEnumerable<CombatPlayerDto>> GetByCombatIdAsync(int combatId)
    {
        var combatPlayers = await _repository.GetByCombatIdAsync(combatId);
        var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(combatPlayers);

        return map;
    }

    private static void CheckParams(CombatPlayerDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.PlayerId, nameof(item.PlayerId));

        ArgumentOutOfRangeException.ThrowIfNegative(item.AverageItemLevel, nameof(item.AverageItemLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(item.ResourcesRecovery, nameof(item.ResourcesRecovery));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageTaken, nameof(item.DamageTaken));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CombatId, 1, nameof(item.CombatId));
    }
}
