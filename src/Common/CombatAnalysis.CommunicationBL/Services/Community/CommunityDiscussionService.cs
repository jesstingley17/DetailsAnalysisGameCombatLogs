using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityDiscussionService(IGenericRepository<CommunityDiscussion, int> repository, IMapper mapper) : IService<CommunityDiscussionDto, int>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDiscussionDto?> CreateAsync(CommunityDiscussionDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityDiscussion>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDiscussionDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityDiscussionDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(id, item.Id);

        CheckParams(item);
        
        var map = _mapper.Map<CommunityDiscussion>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityDiscussionDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(allData);

        return result;
    }

    public async Task<CommunityDiscussionDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDiscussionDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDiscussionDto>> GetByParamAsync<TValue>(Expression<Func<CommunityDiscussionDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityDiscussion, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(result);

        return resultMap;
    }

    private static void CheckParams(CommunityDiscussionDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Title, nameof(item.Title));
        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CommunityId, 1, nameof(item.CommunityId));
    }
}
