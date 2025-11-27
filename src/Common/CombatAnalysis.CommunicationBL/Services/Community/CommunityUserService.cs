using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityUserService(IGenericRepository<CommunityUser, string> repository, IMapper mapper) : IService<CommunityUserDto, string>
{
    private readonly IGenericRepository<CommunityUser, string> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityUserDto?> CreateAsync(CommunityUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityUserDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(string id, CommunityUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityUser>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CommunityUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityUserDto>>(allData);

        return result;
    }

    public async Task<CommunityUserDto?> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityUserDto>> GetByParamAsync<TValue>(Expression<Func<CommunityUserDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityUser, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityUserDto>>(result);

        return resultMap;
    }

    private static void CheckParams(CommunityUserDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Id, nameof(item.Id));
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));

        ArgumentOutOfRangeException.ThrowIfLessThan(item.CommunityId, 1, nameof(item.CommunityId));
    }
}
