using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Exceptions;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;
using System.Transactions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class GroupChatService(IGenericRepository<GroupChat, int> repository, IMapper mapper, IGroupChatMessageService<GroupChatMessageDto, int> groupChatMessageService,
    IServiceTransaction<GroupChatUserDto, string> groupChatUserService, IService<GroupChatRulesDto, int> groupChatRulesService) : IService<GroupChatDto, int>
{
    private readonly IGenericRepository<GroupChat, int> _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IGroupChatMessageService<GroupChatMessageDto, int> _groupChatMessageService = groupChatMessageService;
    private readonly IServiceTransaction<GroupChatUserDto, string> _groupChatUserService = groupChatUserService;
    private readonly IService<GroupChatRulesDto, int> _groupChatRulesService = groupChatRulesService;

    public async Task<GroupChatDto?> CreateAsync(GroupChatDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            throw new BusinessValidationException("Chat name is required.");
        }

        var map = _mapper.Map<GroupChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await DeleteGroupChatMessagesAsync(id);
        await DeleteGroupChatUsersAsync(id);
        await DeleteGroupChatRulesAsync(id);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<GroupChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatDto>>(allData);

        return result;
    }

    public async Task<GroupChatDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatDto>> GetByParamAsync<TValue>(Expression<Func<GroupChatDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<GroupChat, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<GroupChatDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(GroupChatDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            throw new BusinessValidationException("Chat name is required.");
        }

        var map = _mapper.Map<GroupChat>(item);
        await _repository.UpdateAsync(map);
    }

    private async Task DeleteGroupChatMessagesAsync(int chatId)
    {
        var groupChatMessages = await _groupChatMessageService.GetByParamAsync(u => u.ChatId, chatId);
        foreach (var item in groupChatMessages)
        {
            await _groupChatMessageService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteGroupChatUsersAsync(int chatId)
    {
        var groupChatUsers = await _groupChatUserService.GetByParamAsync(u => u.ChatId, chatId);
        foreach (var item in groupChatUsers)
        {
            await _groupChatUserService.DeleteUseExistTransactionAsync(item.Id);
        }
    }

    private async Task DeleteGroupChatRulesAsync(int chatId)
    {
        var groupChatRules = await _groupChatRulesService.GetByParamAsync(u => u.ChatId, chatId);
        foreach (var item in groupChatRules)
        {
            await _groupChatRulesService.DeleteAsync(item.Id);
        }
    }
}
