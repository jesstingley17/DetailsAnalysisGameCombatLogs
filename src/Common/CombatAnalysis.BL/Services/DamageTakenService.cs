using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenService(ICreateBatchRepository<DamageTaken> repository, IMapper mapper) : ICreateBatchService<DamageTakenDto>
{
    private readonly ICreateBatchRepository<DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageTakenDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageTaken>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
