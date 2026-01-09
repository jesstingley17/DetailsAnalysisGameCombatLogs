using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class BossService(IBossRepository repository, IMapper mapper) : IBossService
{
    private readonly IBossRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BossDto?> GetById(int bossId, CancellationToken cancellationToken)
    {
        var boss = await _repository.GetById(bossId, cancellationToken);
        var result = _mapper.Map<BossDto>(boss);

        return result;
    }

    public async Task<BossDto?> GetAsync(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken)
    {
        var boss = await _repository.GetAsync(gameBossId, difficult, groupSize, cancellationToken);
        var result = _mapper.Map<BossDto>(boss);

        return result;
    }
}
