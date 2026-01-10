using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<PlayerDto> GetByGameIdAsync(string gameId, CancellationToken cancellationToken);

    Task<PlayerDto> CreateAsync(PlayerDto item, CancellationToken cancellationToken);
}
