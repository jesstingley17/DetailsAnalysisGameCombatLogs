using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityDiscussionCommentService(IGenericRepository<CommunityDiscussionComment, int> repository, IMapper mapper) : IService<CommunityDiscussionCommentDto, int>
{
    private readonly IGenericRepository<CommunityDiscussionComment, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDiscussionCommentDto?> CreateAsync(CommunityDiscussionCommentDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityDiscussionComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDiscussionCommentDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityDiscussionCommentDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityDiscussionComment>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityDiscussionCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(allData);

        return result;
    }

    public async Task<CommunityDiscussionCommentDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDiscussionCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDiscussionCommentDto>> GetByParamAsync<TValue>(Expression<Func<CommunityDiscussionCommentDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityDiscussionComment, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(result);

        return resultMap;
    }

    private static void CheckParams(CommunityDiscussionCommentDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CommunityDiscussionId, 1, nameof(item.CommunityDiscussionId));
    }
}
