using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using System.Transactions;

namespace Chat.Application.Services;

internal class GroupChatService(IGroupChatRepository repository, IMapper mapper) : IGroupChatService
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatDto> CreateAsync(GroupChatDto createChat)
    {
        var chat = new GroupChat(createChat.Name, createChat.OwnerId);

        var createdItem = await _repository.CreateAsync(chat);

        return createdItem.ToDTO(_mapper);
    }

    public async Task UpdateChatAsync(GroupChatId id,
                                     string? newName,
                                     string? newOwnerId)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (!string.IsNullOrEmpty(newName))
        {
            entity.UpdateName(newName);
        }

        if (!string.IsNullOrEmpty(newOwnerId))
        {
            entity.PassOwner(newOwnerId);
        }

        await _repository.SaveChangesAsync();
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

    public async Task AddRulesAsync(GroupChatRulesDto item)
    {
        await _repository.AddRulesAsync(item.ToEntity(_mapper));
    }

    public async Task UpdateRulesAsync(GroupChatRulesDto item)
    {
        await _repository.UpdateRulesAsync(item.ToEntity(_mapper));
    }

    public async Task<GroupChatRulesDto> GetRulesAsync(int chatId)
    {
        var chat = await _repository.GetByIdAsync(chatId)
                 ?? throw new GroupChatNotFoundException(chatId);

        var rules = chat.Rules
                ?? throw new GroupChatRulesNotFoundException(0);

        return rules.ToDTO(_mapper);
    }
}
