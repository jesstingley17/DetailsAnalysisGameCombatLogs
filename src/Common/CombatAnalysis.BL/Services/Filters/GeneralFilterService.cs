using AutoMapper;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;

namespace CombatAnalysis.BL.Services.Filters;

internal class GeneralFilterService<TModel, TModelMap>(IGeneralFilterRepository<TModelMap> repository, IMapper mapper) : IGeneralFilterService<TModel>
    where TModel : class, IGeneralFilterEntity
    where TModelMap : class, IGeneralFilterEntity
{
    private readonly IMapper _mapper = mapper;
    private readonly IGeneralFilterRepository<TModelMap> _repository = repository;

    public async Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);

        var result = await _repository.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return result;
    }

    public async Task<int> CountTargetsByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));

        var count = await _repository.CountTargetByCombatPlayerIdAsync(combatPlayerId, target, cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegative(page, nameof(page));
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize, nameof(pageSize));

        var result = await _repository.GetByTargetAsync(combatPlayerId, target, page, pageSize, cancellationToken);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }

    public async Task<int> GetTargetValueByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));

        var result = await _repository.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target, cancellationToken);

        return result;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);

        var result = await _repository.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken   );

        return result;
    }

    public async Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));

        var count = await _repository.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator, cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentOutOfRangeException.ThrowIfNegative(page, nameof(page));
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize, nameof(pageSize));

        var result = await _repository.GetByCreatorAsync(combatPlayerId, creator, page, pageSize, cancellationToken);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<string>> GetSpellNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);

        var result = await _repository.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return result;
    }

    public async Task<int> CountSpellByCombatPlayerIdAsync(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));

        var count = await _repository.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell, cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(combatPlayerId, 1);
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(page, nameof(page));
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize, nameof(pageSize));

        var result = await _repository.GetBySpellAsync(combatPlayerId, spell, page, pageSize, cancellationToken);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
