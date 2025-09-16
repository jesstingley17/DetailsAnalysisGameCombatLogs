using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using System.Transactions;

namespace Chat.Application.Services;

internal class PersonalChatService(IPersonalChatRepository repository, IMapper mapper) : IService<PersonalChatDto, int>
{
    private readonly IPersonalChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PersonalChatDto> CreateAsync(PersonalChatDto createChat)
    {
        var chat = new PersonalChat(createChat.InitiatorId, createChat.CompanionId);

        var createdItem = await _repository.CreateAsync(chat);

        return createdItem.ToDTO(_mapper);
    }

    public async Task DeleteAsync(int id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<PersonalChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<PersonalChatDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new PersonalChatNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task UpdateAsync(PersonalChatDto updated)
    {
        if (updated.CompanionUnreadMessages != null)
        {
            await _repository.UpdateCompanionUnreadMessageCountAsync(updated.Id, updated.CompanionUnreadMessages.Value);
        }

        if (updated.InitiatorUnreadMessages != null)
        {
            await _repository.UpdateInitiatorUnreadMessageCountAsync(updated.Id, updated.InitiatorUnreadMessages.Value);
        }
    }
}
