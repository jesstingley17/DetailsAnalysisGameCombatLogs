using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Exceptions;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class PersonalChatMessageService(IPersonalChatMessageRepository<PersonalChatMessage, int> repository, IMapper mapper) : IPersonalChatMessageService<PersonalChatMessageDto, int>
{
    private readonly IPersonalChatMessageRepository<PersonalChatMessage, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PersonalChatMessageDto?> CreateAsync(PersonalChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new BusinessValidationException("Personal chat content is required.");
        }
        
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new BusinessValidationException("Personal chat username is required.");
        }

        var map = _mapper.Map<PersonalChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<PersonalChatMessageDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(allData);

        return result;
    }

    public async Task<PersonalChatMessageDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<PersonalChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetByParamAsync<TValue>(Expression<Func<PersonalChatMessageDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<PersonalChatMessage, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int pageSize = 100)
    {
        var result = await _repository.GetByChatIdAsyn(chatId, pageSize);
        var map = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

        return map;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetMoreByChatIdAsync(int chatId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByChatIdAsyn(chatId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

        return map;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _repository.CountByChatIdAsync(chatId);

        return count;
    }

    public async Task UpdateAsync(PersonalChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new BusinessValidationException("Personal chat content is required.");
        }

        if (string.IsNullOrEmpty(item.Username))
        {
            throw new BusinessValidationException("Personal chat username is required.");
        }

        var map = _mapper.Map<PersonalChatMessage>(item);
        await _repository.UpdateAsync(map);
    }
}
