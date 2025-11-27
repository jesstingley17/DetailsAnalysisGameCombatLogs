using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostLikeService(IGenericRepository<UserPostLike, int> repository, IMapper mapper) : IService<UserPostLikeDto, int>
{
    private readonly IGenericRepository<UserPostLike, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostLikeDto?> CreateAsync(UserPostLikeDto item)
    {
        var map = _mapper.Map<UserPostLike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostLikeDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserPostLikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostLikeDto>>(allData);

        return result;
    }

    public async Task<UserPostLikeDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostLikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostLikeDto>> GetByParamAsync<TValue>(Expression<Func<UserPostLikeDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<UserPostLike, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<UserPostLikeDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(int id, UserPostLikeDto item)
    {
        var map = _mapper.Map<UserPostLike>(item);
        await _repository.UpdateAsync(id, map);
    }
}