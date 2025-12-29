using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class SpecializationScoreService(ISpecScore specRepository, IGenericRepositoryBatch<SpecializationScore> repository, IMapper mapper) : QueryService<SpecializationScoreDto, SpecializationScore>(repository, mapper), IMutationServiceBatch<SpecializationScoreDto>, ISpecScoreService
{
    private readonly IGenericRepositoryBatch<SpecializationScore> _repository = repository;
    private readonly ISpecScore _specRepository = specRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<SpecializationScoreDto> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<SpecializationScore>>(items);
        await _repository.CreateBatchAsync(map);
    }

    public async Task<SpecializationScoreDto> CreateAsync(SpecializationScoreDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<SpecializationScore>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<SpecializationScoreDto>(createdItem);

        return resultMap;
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

    public async Task<IEnumerable<SpecializationScoreDto>> GetBySpecIdAsync(int specId, int bossId)
    {
        var result = await _specRepository.GetBySpecIdAsync(specId, bossId);
        var resultMap = _mapper.Map<IEnumerable<SpecializationScoreDto>>(result);

        return resultMap;
    }

    private static void CheckParams(SpecializationScoreDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecId, nameof(item.SpecId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.BossId, nameof(item.BossId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Damage, nameof(item.Damage));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Heal, nameof(item.Heal));
    }
}
