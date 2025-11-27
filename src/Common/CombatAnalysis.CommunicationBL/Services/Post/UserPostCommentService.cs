using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostCommentService(IGenericRepository<UserPostComment, int> repository, IMapper mapper) : IService<UserPostCommentDto, int>
{
    private readonly IGenericRepository<UserPostComment, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostCommentDto?> CreateAsync(UserPostCommentDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostCommentDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, UserPostCommentDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPostComment>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserPostCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostCommentDto>>(allData);

        return result;
    }

    public async Task<UserPostCommentDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostCommentDto>> GetByParamAsync<TValue>(Expression<Func<UserPostCommentDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<UserPostComment, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<UserPostCommentDto>>(result);

        return resultMap;
    }

    private static void CheckParams(UserPostCommentDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.UserPostId, nameof(item.UserPostId));

        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
