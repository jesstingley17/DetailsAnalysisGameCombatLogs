using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Filters;

namespace CombatAnalysis.BL.Services.Filters;

internal class DamageFilterService(IDamageFilterRepository repository, IMapper mapper) : IDamageFilterService
{
    private readonly IMapper _mapper = mapper;
    private readonly IDamageFilterRepository _repository = repository;

    public async Task<IEnumerable<List<CombatTargetDto>>> GetDamageByEachTargetAsync(int combatId)
    {
        var damageByEachTarget = await _repository.GetDamageByEachTargetAsync(combatId);
        var damageByEachTargetMap = _mapper.Map<IEnumerable<List<CombatTargetDto>>>(damageByEachTarget);

        return damageByEachTargetMap;
    }
}
