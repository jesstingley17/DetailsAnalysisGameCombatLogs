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

internal class GroupChatUserService(IGenericRepository<GroupChatUser, string> repository, IService<UnreadGroupChatMessageDto, int> unreadGroupChatMessageService, IMapper mapper) : IServiceTransaction<GroupChatUserDto, string>
{
    private readonly IGenericRepository<GroupChatUser, string> _repository = repository;
    private readonly IService<UnreadGroupChatMessageDto, int> _unreadGroupChatMessageService = unreadGroupChatMessageService;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatUserDto?> CreateAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Username))
        {
            throw new BusinessValidationException("Group chat user username is required.");
        }

        item.Id = Guid.NewGuid().ToString();

        var map = _mapper.Map<GroupChatUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(string id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await DeleteUnreadGroupChatMessageAsync(id);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task DeleteUseExistTransactionAsync(string id)
    {
        await DeleteUnreadGroupChatMessageAsync(id);

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatUserDto>>(allData);

        return result;
    }

    public async Task<GroupChatUserDto?> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<GroupChatUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetByParamAsync<TValue>(Expression<Func<GroupChatUserDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<GroupChatUser, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<GroupChatUserDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Username))
        {
            throw new BusinessValidationException("Group chat user username is required.");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        await _repository.UpdateAsync(map);
    }

    private async Task DeleteUnreadGroupChatMessageAsync(string userId)
    {
        var unreadGroupChatMessage = await _unreadGroupChatMessageService.GetByParamAsync(u => u.GroupChatUserId, userId);
        foreach (var item in unreadGroupChatMessage)
        {
            await _unreadGroupChatMessageService.DeleteAsync(item.Id);
        }
    }
}
