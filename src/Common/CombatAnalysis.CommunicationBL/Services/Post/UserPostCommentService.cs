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
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto),
                $"The property {nameof(UserPostCommentDto.Content)} of the {nameof(UserPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostCommentDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
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

    public async Task UpdateAsync(int id, UserPostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto),
                $"The property {nameof(UserPostCommentDto.Content)} of the {nameof(UserPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPostComment>(item);
        await _repository.UpdateAsync(id, map);
    }
}
