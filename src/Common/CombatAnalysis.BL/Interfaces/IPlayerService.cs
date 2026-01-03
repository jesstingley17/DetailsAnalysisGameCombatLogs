using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> GetByIdAsync(string id);

    Task<PlayerDto> GetByGameIdAsync(string gameId);

    Task<PlayerDto> CreateAsync(PlayerDto item);
}
