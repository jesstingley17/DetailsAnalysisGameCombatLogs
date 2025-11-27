using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class InviteToCommunityService(IGenericRepository<InviteToCommunity, int> repository, IMapper mapper) : IService<InviteToCommunityDto, int>
{
    private readonly IGenericRepository<InviteToCommunity, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<InviteToCommunityDto?> CreateAsync(InviteToCommunityDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<InviteToCommunity>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<InviteToCommunityDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, InviteToCommunityDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<InviteToCommunity>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<InviteToCommunityDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<InviteToCommunityDto>>(allData);

        return result;
    }

    public async Task<InviteToCommunityDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<InviteToCommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<InviteToCommunityDto>> GetByParamAsync<TValue>(Expression<Func<InviteToCommunityDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<InviteToCommunity, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<InviteToCommunityDto>>(result);

        return resultMap;
    }

    private static void CheckParams(InviteToCommunityDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));
        ArgumentOutOfRangeException.ThrowIfLessThan(item.CommunityId, 1, nameof(item.CommunityId));

        ArgumentException.ThrowIfNullOrEmpty(item.ToAppUserId, nameof(item.ToAppUserId));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
