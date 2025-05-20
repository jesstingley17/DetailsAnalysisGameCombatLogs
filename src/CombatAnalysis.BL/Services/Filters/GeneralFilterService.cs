using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;

namespace CombatAnalysis.BL.Services.Filters;

internal class GeneralFilterService<TModel, TModelMap>(IGeneralFilter<TModelMap> repository, IMapper mapper) : IGeneralFilterService<TModel>
    where TModel : class, IGeneralFilterEntity
    where TModelMap : class, IGeneralFilterEntity
{
    private readonly IMapper _mapper = mapper;
    private readonly IGeneralFilter<TModelMap> _repository = repository;

    public async Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

        return result;
    }

    public async Task<int> CountTargetsByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var count = await _repository.CountTargetByCombatPlayerIdAsync(combatPlayerId, target);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetTargetByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var result = await _repository.GetTargetByCombatPlayerIdAsync(combatPlayerId, target, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<List<CombatTargetDto>>> GetDamageByEachTargetAsync(int combatId)
    {
        var damageByEachTarget = await _repository.GetDamageByEachTargetAsync(combatId);
        var damageByEachTargetMap = _mapper.Map<IEnumerable<List<CombatTargetDto>>>(damageByEachTarget);

        return damageByEachTargetMap;
    }

    public async Task<int> GetTargetValueByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var result = await _repository.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target);

        return result;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId);

        return result;
    }

    public async Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator)
    {
        var count = await _repository.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, int page, int pageSize)
    {
        var result = await _repository.GetCreatorByCombatPlayerIdAsync(combatPlayerId, creator, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<string>> GetSpellNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId);

        return result;
    }

    public async Task<int> CountSpellByCombatPlayerIdAsync(int combatPlayerId, string spell)
    {
        var count = await _repository.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetSpellByCombatPlayerIdAsync(int combatPlayerId, string spell, int page, int pageSize)
    {
        var result = await _repository.GetSpellByCombatPlayerIdAsync(combatPlayerId, spell, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
