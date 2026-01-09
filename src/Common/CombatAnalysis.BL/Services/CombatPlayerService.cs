using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerService(ICombatPlayerRepository repository, IMapper mapper) : ICombatPlayerService
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(IEnumerable<CombatPlayerDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<CombatPlayer>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }

    public async Task<IEnumerable<CombatPlayerDto>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayers = await _repository.GetByCombatIdAsync(combatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(combatPlayers);

        return map;
    }
}
