using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneService(ICreateBatchRepository<DamageDone> repository, IMapper mapper) : ICreateBatchService<DamageDoneDto>
{
    private readonly ICreateBatchRepository<DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageDoneDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageDone>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
