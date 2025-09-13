using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Repositories;
using System.Transactions;

namespace Chat.Application.Services;

internal class GroupChatService(IGroupChatRepository repository, IMapper mapper) : IService<GroupChatDto, int>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatDto?> CreateAsync(GroupChatDto item)
    {
        var createdItem = await _repository.CreateAsync(item.ToEntity(_mapper));
        if (createdItem == null)
        {
            return null;
        }

        return createdItem.ToDTO(_mapper);
    }

    public async Task DeleteAsync(GroupChatDto item)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _repository.DeleteAsync(item.ToEntity(_mapper));

        scope.Complete();
    }

    public async Task<IEnumerable<GroupChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<GroupChatDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        return result.ToDTO(_mapper);
    }

    public async Task UpdateAsync(GroupChatDto item)
    {
        await _repository.UpdateAsync(item.ToEntity(_mapper));
    }
}
