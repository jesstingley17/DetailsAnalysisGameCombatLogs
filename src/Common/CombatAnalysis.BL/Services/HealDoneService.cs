using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneService(ICreateBatchRepository<HealDone> repository, IMapper mapper) : ICreateBatchService<HealDoneDto>
{
    private readonly ICreateBatchRepository<HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<HealDoneDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<HealDone>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
