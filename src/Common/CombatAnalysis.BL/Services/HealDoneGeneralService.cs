using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneGeneralService(ICreateBatchRepository<HealDoneGeneral> repository, IMapper mapper) : ICreateBatchService<HealDoneGeneralDto>
{
    private readonly ICreateBatchRepository<HealDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<HealDoneGeneralDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<HealDoneGeneral>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
