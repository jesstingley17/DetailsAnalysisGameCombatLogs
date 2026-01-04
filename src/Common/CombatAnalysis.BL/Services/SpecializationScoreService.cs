using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class SpecializationScoreService(ISpecializationScoreRepository repository, IMapper mapper) : ISpecializationScoreService
{
    private readonly ISpecializationScoreRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<SpecializationScoreDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<SpecializationScore>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<int> UpdateAsync(SpecializationScoreDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<SpecializationScore>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    public async Task<SpecializationScoreDto?> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var spec = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var map = _mapper.Map<SpecializationScoreDto>(spec);

        return map;
    }

    private static void CheckParams(SpecializationScoreDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecializationId, nameof(item.SpecializationId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageScore, nameof(item.DamageScore));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageScore));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealScore, nameof(item.HealScore));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealScore));
    }
}
