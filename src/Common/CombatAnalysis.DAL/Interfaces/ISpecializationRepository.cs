using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecializationRepository
{
    Task<Specialization?> GetBySpellsAsync(string spells);
}
