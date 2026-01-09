using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerPositionService(ICreateBatchRepository<CombatPlayerPosition> repository, IMapper mapper) : QueryService<CombatPlayerPositionDto, CombatPlayerPosition>(repository, mapper), ICreateBatchService<CombatPlayerPositionDto>
{
    private readonly ICreateBatchRepository<CombatPlayerPosition> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<CombatPlayerPositionDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<CombatPlayerPosition>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
