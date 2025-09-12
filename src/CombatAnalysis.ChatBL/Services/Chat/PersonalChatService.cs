using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;
using System.Transactions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class PersonalChatService(IGenericRepository<PersonalChat, int> repository, IMapper mapper,
    IPersonalChatMessageService<PersonalChatMessageDto, int> personalChatMessageService) : IService<PersonalChatDto, int>
{
    private readonly IGenericRepository<PersonalChat, int> _repository = repository;
    private readonly IPersonalChatMessageService<PersonalChatMessageDto, int> _personalChatMessageService = personalChatMessageService;
    private readonly IMapper _mapper = mapper;

    public async Task<PersonalChatDto?> CreateAsync(PersonalChatDto item)
    {
        var map = _mapper.Map<PersonalChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<PersonalChatDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await DeletePersonalChatMessagesAsync(id);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<PersonalChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PersonalChatDto>>(allData);

        return result;
    }

    public async Task<PersonalChatDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<PersonalChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatDto>> GetByParamAsync<TValue>(Expression<Func<PersonalChatDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<PersonalChat, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<PersonalChatDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(PersonalChatDto item)
    {
        var map = _mapper.Map<PersonalChat>(item);
        await _repository.UpdateAsync(map);
    }

    private async Task DeletePersonalChatMessagesAsync(int chatId)
    {
        var perosnalChatMessages = await _personalChatMessageService.GetByParamAsync(u => u.ChatId, chatId);
        foreach (var item in perosnalChatMessages)
        {
            await _personalChatMessageService.DeleteAsync(item.Id);
        }
    }
}
