using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class BestSpecializationScoreService(IBestSpecializationScoreRepository repository, IMapper mapper) : IBestSpecializationScoreService
{
    private readonly IBestSpecializationScoreRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<int> UpdateAsync(BestSpecializationScoreDto item, CancellationToken cancellationToken)
    {
        CheckParams(item);

        var map = _mapper.Map<BestSpecializationScore>(item);
        var rowsAffected = await _repository.UpdateAsync(map.Id, map, cancellationToken);

        return rowsAffected;
    }

    public async Task<BestSpecializationScoreDto?> GetAsync(int specializationId, int bossId, CancellationToken cancellationToken)
    {
        var bestSpecScore = await _repository.GetAsync(specializationId, bossId, cancellationToken);
        var map = _mapper.Map<BestSpecializationScoreDto>(bestSpecScore);

        return map;
    }

    private static void CheckParams(BestSpecializationScoreDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.DamageDone, nameof(item.DamageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.HealDone, nameof(item.HealDone));
        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecializationId, nameof(item.SpecializationId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.BossId, nameof(item.BossId));
    }
}
