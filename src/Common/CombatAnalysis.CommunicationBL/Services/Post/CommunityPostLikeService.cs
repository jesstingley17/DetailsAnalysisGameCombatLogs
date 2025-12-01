using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostLikeService(IGenericRepository<CommunityPostLike, int> repository, IMapper mapper) : IService<CommunityPostLikeDto, int>
{
    private readonly IGenericRepository<CommunityPostLike, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostLikeDto?> CreateAsync(CommunityPostLikeDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityPostLike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostLikeDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityPostLikeDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(id, item.Id);

        CheckParams(item);

        var map = _mapper.Map<CommunityPostLike>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityPostLikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostLikeDto>>(allData);

        return result;
    }

    public async Task<CommunityPostLikeDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostLikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostLikeDto>> GetByParamAsync<TValue>(Expression<Func<CommunityPostLikeDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityPostLike, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityPostLikeDto>>(result);

        return resultMap;
    }

    private static void CheckParams(CommunityPostLikeDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.CommunityPostId, nameof(item.CommunityPostId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.CommunityId, nameof(item.CommunityId));

        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
