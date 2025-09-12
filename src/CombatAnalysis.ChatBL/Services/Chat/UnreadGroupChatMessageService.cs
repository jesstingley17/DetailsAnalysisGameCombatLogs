using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class UnreadGroupChatMessageService(IGenericRepository<UnreadGroupChatMessage, int> repository, IMapper mapper) : IService<UnreadGroupChatMessageDto, int>
{
    private readonly IGenericRepository<UnreadGroupChatMessage, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<UnreadGroupChatMessageDto?> CreateAsync(UnreadGroupChatMessageDto item)
    {
        var map = _mapper.Map<UnreadGroupChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<UnreadGroupChatMessageDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UnreadGroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<UnreadGroupChatMessageDto>>(allData);

        return result;
    }

    public async Task<UnreadGroupChatMessageDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<UnreadGroupChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UnreadGroupChatMessageDto>> GetByParamAsync<TValue>(Expression<Func<UnreadGroupChatMessageDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<UnreadGroupChatMessage, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<UnreadGroupChatMessageDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(UnreadGroupChatMessageDto item)
    {
        var map = _mapper.Map<UnreadGroupChatMessage>(item);
        await _repository.UpdateAsync(map);
    }
}
