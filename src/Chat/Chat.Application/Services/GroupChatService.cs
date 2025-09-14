using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using System.Transactions;

namespace Chat.Application.Services;

internal class GroupChatService(IGroupChatRepository repository, IMapper mapper) : IService<GroupChatDto, int>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatDto> CreateAsync(GroupChatDto createChat)
    {
        var chat = new GroupChat(createChat.Id, createChat.Name, createChat.OwnerId);

        var createdItem = await _repository.CreateAsync(chat);

        return createdItem.ToDTO(_mapper);
    }

    public async Task DeleteAsync(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<GroupChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<GroupChatDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new GroupChatNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task UpdateAsync(GroupChatDto item)
    {
        await _repository.UpdateAsync(item.ToEntity(_mapper));
    }
}
