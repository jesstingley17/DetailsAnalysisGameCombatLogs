using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Exceptions;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class GroupChatMessageService(IGroupChatMessageRepository<int> repository, IMapper mapper) : IGroupChatMessageService<GroupChatMessageDto, int>
{
    private readonly IGroupChatMessageRepository<int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatMessageDto?> CreateAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Message))
        {
            throw new BusinessValidationException("Group chat message content is required.");
        }

        if (string.IsNullOrWhiteSpace(item.Username))
        {
            throw new BusinessValidationException("Group chat message username is required.");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatMessageDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatMessageDto>>(allData);

        return result;
    }

    public async Task<GroupChatMessageDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetByParamAsync<TValue>(Expression<Func<GroupChatMessageDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<GroupChatMessage, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, string groupChatUserId, int pageSize = 100)
    {
        var result = await _repository.GetByChatIdAsyn(chatId, groupChatUserId, pageSize);
        var map = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

        return map;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetMoreByChatIdAsync(int chatId, string groupChatUserId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByChatIdAsyn(chatId, groupChatUserId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

        return map;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _repository.CountByChatIdAsync(chatId);

        return count;
    }

    public async Task UpdateAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Message))
        {
            throw new BusinessValidationException("Group chat message content is required.");
        }

        if (string.IsNullOrWhiteSpace(item.Username))
        {
            throw new BusinessValidationException("Group chat message username is required.");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        await _repository.UpdateAsync(map);
    }
}
