using AutoMapper;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.BL.Services.General;

internal class PlayerInfoPaginationService<TModel, TModelMap>(IPlayerInfoPaginationRepository<TModelMap> playerInfoPaginationRepository, IPlayerInfoRepository<TModelMap> playerInfoRepository, IMapper mapper) : PlayerInfoService<TModel, TModelMap>(playerInfoRepository, mapper), IPlayerInfoPaginationService<TModel>
    where TModel : class
    where TModelMap : class, IEntity, ICombatPlayerEntity
{
    private readonly IPlayerInfoPaginationRepository<TModelMap> _playerInfoPaginationRepository = playerInfoPaginationRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(combatPlayerId, nameof(combatPlayerId));
        ArgumentOutOfRangeException.ThrowIfNegative(page, nameof(page));
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize, nameof(pageSize));

        var result = await _playerInfoPaginationRepository.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
