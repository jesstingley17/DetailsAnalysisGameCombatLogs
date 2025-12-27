using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class BossService(IBossRepository repository, IMapper mapper) : IBossService
{
    private readonly IBossRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BossDto?> GetAsync(int gameBossId, int difficult, int groupSize)
    {
        var boss = await _repository.GetAsync(gameBossId, difficult, groupSize);
        var result = _mapper.Map<BossDto>(boss);

        return result;
    }
}
