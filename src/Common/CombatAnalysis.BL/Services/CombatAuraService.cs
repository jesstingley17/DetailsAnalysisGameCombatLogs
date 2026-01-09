using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatAuraService(ICreateBatchRepository<CombatAura> repository, IMapper mapper) : QueryService<CombatAuraDto, CombatAura>(repository, mapper), ICreateBatchService<CombatAuraDto>
{
    private readonly ICreateBatchRepository<CombatAura> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<CombatAuraDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<CombatAura>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
