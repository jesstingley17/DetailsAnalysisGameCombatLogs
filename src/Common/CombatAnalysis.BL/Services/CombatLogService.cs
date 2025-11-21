using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatLogService(IGenericRepository<CombatLog> userRepository, IMapper mapper) : QueryService<CombatLogDto, CombatLog>(userRepository, mapper), IMutationService<CombatLogDto>
{
    private readonly IGenericRepository<CombatLog> _repository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatLogDto> CreateAsync(CombatLogDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLog>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatLogDto>(createdItem);

        return resultMap;
    }

    public async Task<int> UpdateAsync(CombatLogDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLog>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var entityDeleted = await _repository.DeleteAsync(id);

        return entityDeleted;
    }

    private static void CheckParams(CombatLogDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Name, nameof(item.Name));

        ArgumentOutOfRangeException.ThrowIfNegative(item.LogType, nameof(item.LogType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.NumberReadyCombats, nameof(item.NumberReadyCombats));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CombatsInQueue, nameof(item.CombatsInQueue));

        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
