using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerDeathService(ICreateBatchRepository<CombatPlayerDeath> repository, IMapper mapper) : QueryService<CombatPlayerDeathDto, CombatPlayerDeath>(repository, mapper), ICreateBatchService<CombatPlayerDeathDto>
{
    private readonly ICreateBatchRepository<CombatPlayerDeath> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<CombatPlayerDeathDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<CombatPlayerDeath>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
