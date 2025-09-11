using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
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

    public Task<GroupChatUserDto> CreateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        try
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await DeleteUnreadGroupChatMessageAsync(id);

            var rowsAffected = await _repository.DeleteAsync(id);

            scope.Complete();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<int> DeleteUseExistTransactionAsync(string id)
    {
        try
        {
            await DeleteUnreadGroupChatMessageAsync(id);

            var rowsAffected = await _repository.DeleteAsync(id);

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatUserDto>>(allData);

        return result;
    }

    public async Task<GroupChatUserDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
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

    public Task<int> UpdateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatUserDto> CreateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        item.Id = Guid.NewGuid().ToString();

        var map = _mapper.Map<GroupChatUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteUnreadGroupChatMessageAsync(string userId)
    {
        var unreadGroupChatMessage = await _unreadGroupChatMessageService.GetByParamAsync(u => u.GroupChatUserId, userId);
        foreach (var item in unreadGroupChatMessage)
        {
            var rowsAffected = await _unreadGroupChatMessageService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Unread group chat message didn't removed");
            }
        }
    }
}
