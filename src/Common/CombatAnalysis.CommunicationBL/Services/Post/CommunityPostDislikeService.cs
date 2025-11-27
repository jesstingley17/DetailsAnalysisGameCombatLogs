using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostDislikeService(IGenericRepository<CommunityPostDislike, int> repository, IMapper mapper) : IService<CommunityPostDislikeDto, int>
{
    private readonly IGenericRepository<CommunityPostDislike, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostDislikeDto?> CreateAsync(CommunityPostDislikeDto item)
    {
        var map = _mapper.Map<CommunityPostDislike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostDislikeDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityPostDislikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostDislikeDto>>(allData);

        return result;
    }

    public async Task<CommunityPostDislikeDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostDislikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDislikeDto>> GetByParamAsync<TValue>(Expression<Func<CommunityPostDislikeDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityPostDislike, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityPostDislikeDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityPostDislikeDto item)
    {
        var map = _mapper.Map<CommunityPostDislike>(item);
        await _repository.UpdateAsync(id, map);
    }
}
