using AutoMapper;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.BL.Services.General;

internal class PlayerInfoService<TModel, TModelMap>(IPlayerInfoRepository<TModelMap> playerInfoRepository, IMapper mapper) : IPlayerInfoService<TModel>
    where TModel : class
    where TModelMap : class, IEntity, ICombatPlayerEntity
{
    private readonly IPlayerInfoRepository<TModelMap> _playerInfoRepository = playerInfoRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(combatPlayerId);

        var result = await _playerInfoRepository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
