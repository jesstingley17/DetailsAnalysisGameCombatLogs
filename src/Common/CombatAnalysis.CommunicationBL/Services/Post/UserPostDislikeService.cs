using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostDislikeService(IGenericRepository<UserPostDislike, int> repository, IMapper mapper) : IService<UserPostDislikeDto, int>
{
    private readonly IGenericRepository<UserPostDislike, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostDislikeDto?> CreateAsync(UserPostDislikeDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPostDislike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostDislikeDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, UserPostDislikeDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPostDislike>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserPostDislikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostDislikeDto>>(allData);

        return result;
    }

    public async Task<UserPostDislikeDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostDislikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDislikeDto>> GetByParamAsync<TValue>(Expression<Func<UserPostDislikeDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<UserPostDislike, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<UserPostDislikeDto>>(result);

        return resultMap;
    }

    private static void CheckParams(UserPostDislikeDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.UserPostId, nameof(item.UserPostId));

        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
