using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneGeneralService(ICreateBatchRepository<DamageDoneGeneral> repository, IMapper mapper) : ICreateBatchService<DamageDoneGeneralDto>
{
    private readonly ICreateBatchRepository<DamageDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<DamageDoneGeneralDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<DamageDoneGeneral>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
