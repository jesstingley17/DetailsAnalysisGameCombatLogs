using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostCommentService(IGenericRepository<CommunityPostComment, int> repository, IMapper mapper) : IService<CommunityPostCommentDto, int>
{
    private readonly IGenericRepository<CommunityPostComment, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostCommentDto?> CreateAsync(CommunityPostCommentDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityPostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostCommentDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityPostCommentDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(id, item.Id);

        CheckParams(item);

        var map = _mapper.Map<CommunityPostComment>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityPostCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostCommentDto>>(allData);

        return result;
    }

    public async Task<CommunityPostCommentDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostCommentDto>> GetByParamAsync<TValue>(Expression<Func<CommunityPostCommentDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityPostComment, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityPostCommentDto>>(result);

        return resultMap;
    }

    private static void CheckParams(CommunityPostCommentDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.CommentType, nameof(item.CommentType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CommunityPostId, nameof(item.CommunityPostId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.CommunityId, nameof(item.CommunityId));

        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
