using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Exceptions;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Services;

internal class BannedUserService(IGenericRepository<BannedUser, int> repository, IMapper mapper) : IService<BannedUserDto, int>
{
    private readonly IGenericRepository<BannedUser, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BannedUserDto?> CreateAsync(BannedUserDto item)
    {
        BannedUserException.ThrowIfEquals(item.BannedUserId, item.WhomBannedId);

        var map = _mapper.Map<BannedUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<BannedUserDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<BannedUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<BannedUserDto>>(allData);

        return result;
    }

    public async Task<BannedUserDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<BannedUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<BannedUserDto>> GetByParamAsync<TValue>(Expression<Func<BannedUserDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<BannedUser, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<BannedUserDto>>(result);

        return resultMap;
    }
}
