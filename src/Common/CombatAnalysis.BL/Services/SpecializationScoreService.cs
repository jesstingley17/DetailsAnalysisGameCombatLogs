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

    public Task<SpecializationScoreDto> CreateAsync(SpecializationScoreDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(SpecializationScoreDto), $"The {nameof(SpecializationScoreDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var affectedRows = await _repository.DeleteAsync(id);

        return affectedRows;
    }

    public Task<int> UpdateAsync(SpecializationScoreDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(SpecializationScoreDto), $"The {nameof(SpecializationScoreDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public async Task<IEnumerable<SpecializationScoreDto>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var result = await _specRepository.GetBySpecIdAsync(specId, bossId, difficult);
        var resultMap = _mapper.Map<IEnumerable<SpecializationScoreDto>>(result);

        return resultMap;
    }

    private async Task<SpecializationScoreDto> CreateInternalAsync(SpecializationScoreDto item)
    {
        var map = _mapper.Map<SpecializationScore>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<SpecializationScoreDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(SpecializationScoreDto item)
    {
        var map = _mapper.Map<SpecializationScore>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
