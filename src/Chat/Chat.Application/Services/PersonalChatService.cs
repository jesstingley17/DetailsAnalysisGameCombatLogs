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

internal class PersonalChatService(IPersonalChatRepository repository, IMapper mapper) : IPersonalChatService
{
    private readonly IPersonalChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PersonalChatDto> CreateAsync(PersonalChatDto createChat)
    {
        var chat = new PersonalChat(createChat.InitiatorId, createChat.CompanionId);

        var createdItem = await _repository.CreateAsync(chat);

        return createdItem.ToDTO(_mapper);
    }

    public async Task UpdateChatAsync(PersonalChatId id,
                                         int? initiatorUnreadMessages,
                                         int? companionUnreadMessages)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (initiatorUnreadMessages.HasValue)
        {
            entity.UpdateInitiatorUnreadMessageCount(initiatorUnreadMessages.Value);
        }

        if (companionUnreadMessages.HasValue)
        {
            entity.UpdateCompanionUnreadMessageCount(companionUnreadMessages.Value);
        }

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<PersonalChatDto>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();

        return result.ToDTOCollection(_mapper);
    }

    public async Task<PersonalChatDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new PersonalChatNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task<IEnumerable<PersonalChatDto>> GetByUserIdAsync(string userId)
    {
        var result = await _repository.GetByUserIdAsync(userId);

        return result.ToDTOCollection(_mapper);
    }
}
