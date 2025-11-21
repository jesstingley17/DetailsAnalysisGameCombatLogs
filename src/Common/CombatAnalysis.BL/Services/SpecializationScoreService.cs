using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class SpecializationScoreService(ISpecScore specRepository, IGenericRepository<SpecializationScore> repository, IMapper mapper) : QueryService<SpecializationScoreDto, SpecializationScore>(repository, mapper), IMutationService<SpecializationScoreDto>, ISpecScoreService
{
    private readonly IGenericRepository<SpecializationScore> _repository = repository;
    private readonly ISpecScore _specRepository = specRepository;
    private readonly IMapper _mapper = mapper;

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

    public async Task<IEnumerable<SpecializationScoreDto>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var result = await _specRepository.GetBySpecIdAsync(specId, bossId, difficult);
        var resultMap = _mapper.Map<IEnumerable<SpecializationScoreDto>>(result);

        return resultMap;
    }

    private static void CheckParams(SpecializationScoreDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentOutOfRangeException.ThrowIfNegative(item.SpecId, nameof(item.SpecId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.BossId, nameof(item.BossId));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Difficult, nameof(item.Difficult));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Damage, nameof(item.Damage));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Heal, nameof(item.Heal));
    }
}
