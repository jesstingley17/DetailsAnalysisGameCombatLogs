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

    public Task<UnreadGroupChatMessageDto> CreateAsync(UnreadGroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UnreadGroupChatMessageDto), $"The {nameof(UnreadGroupChatMessageDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<UnreadGroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<UnreadGroupChatMessageDto>>(allData);

        return result;
    }

    public async Task<UnreadGroupChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
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

    public Task<int> UpdateAsync(UnreadGroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UnreadGroupChatMessageDto), $"The {nameof(UnreadGroupChatMessageDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<UnreadGroupChatMessageDto> CreateInternalAsync(UnreadGroupChatMessageDto item)
    {
        var map = _mapper.Map<UnreadGroupChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UnreadGroupChatMessageDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UnreadGroupChatMessageDto item)
    {
        var map = _mapper.Map<UnreadGroupChatMessage>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
