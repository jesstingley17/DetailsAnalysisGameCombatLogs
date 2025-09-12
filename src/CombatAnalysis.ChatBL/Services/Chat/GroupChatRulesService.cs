using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class GroupChatRulesService(IGenericRepository<GroupChatRules, int> repository, IMapper mapper) : IService<GroupChatRulesDto, int>
{
    private readonly IGenericRepository<GroupChatRules, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatRulesDto?> CreateAsync(GroupChatRulesDto item)
    {
        var map = _mapper.Map<GroupChatRules>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatRulesDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<GroupChatRulesDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatRulesDto>>(allData);

        return result;
    }

    public async Task<GroupChatRulesDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatRulesDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatRulesDto>> GetByParamAsync<TValue>(Expression<Func<GroupChatRulesDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<GroupChatRules, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<GroupChatRulesDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(GroupChatRulesDto item)
    {
        var map = _mapper.Map<GroupChatRules>(item);
        await _repository.UpdateAsync(map);
    }
}
