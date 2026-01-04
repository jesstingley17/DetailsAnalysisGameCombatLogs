using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ISpecializationService
{
    Task<SpecializationDto?> GetBySpellsAsync(string spells);
}