using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService(ICreateBatchRepository<DamageTakenGeneral> repository, IMapper mapper) : ICreateBatchService<DamageTakenGeneralDto>
{
    private readonly ICreateBatchRepository<DamageTakenGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageTakenGeneralDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageTakenGeneral>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
