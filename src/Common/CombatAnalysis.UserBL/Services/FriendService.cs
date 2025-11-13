using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Exceptions;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Services;

internal class FriendService(IFriendRepository repository, IMapper mapper) : IFriendService
{
    private readonly IFriendRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<FriendDto?> CreateAsync(FriendCreateDto item)
    {
        FriendException.ThrowIfEquals(item.ForWhomId, item.WhoFriendId);

        var map = _mapper.Map<Friend>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<FriendDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<FriendDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<FriendDto>>(allData);

        return result;
    }

    public async Task<FriendDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<FriendDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await _repository.GetByParamAsync(paramName, value);
        var resultMap = _mapper.Map<IEnumerable<FriendDto>>(result);

        return resultMap;
    }
}
