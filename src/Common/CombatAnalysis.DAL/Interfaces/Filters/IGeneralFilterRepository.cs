using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Filters;

public interface IGeneralFilterRepository<TModel>
    where TModel : class, IGeneralFilterEntity
{
    Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountTargetByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> GetTargetValueByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetSpellNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountSpellByCombatPlayerIdAsync(int combatPlayerId, string spell, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken);
}