using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class SpecializationService(ISpecializationRepository repository, IMapper mapper) : ISpecializationService
{
    private readonly ISpecializationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<SpecializationDto?> GetBySpellsAsync(string spells, CancellationToken cancellationToken)
    {
        var result = await _repository.GetBySpellsAsync(spells, cancellationToken);
        var resultMap = _mapper.Map<SpecializationDto>(result);

        return resultMap;
    }
}
