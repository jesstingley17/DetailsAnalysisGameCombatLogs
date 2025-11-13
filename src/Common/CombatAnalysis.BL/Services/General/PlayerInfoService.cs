using AutoMapper;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.BL.Services.General;

internal class PlayerInfoService<TModel, TModelMap>(IPlayerInfoRepository<TModelMap> playerInfoRepository, IMapper mapper) : IPlayerInfoService<TModel>
    where TModel : class
    where TModelMap : class, IEntity
{
    private readonly IPlayerInfoRepository<TModelMap> _playerInfoRepository = playerInfoRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _playerInfoRepository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        var result = await _playerInfoRepository.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
