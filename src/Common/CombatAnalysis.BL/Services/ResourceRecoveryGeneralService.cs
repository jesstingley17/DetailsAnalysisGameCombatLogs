using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService(ICreateBatchRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : ICreateBatchService<ResourceRecoveryGeneralDto>
{
    private readonly ICreateBatchRepository<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateBatchAsync(List<ResourceRecoveryGeneralDto> items, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneral>>(items);
        await _repository.CreateBatchAsync(map, cancellationToken);
    }
}
