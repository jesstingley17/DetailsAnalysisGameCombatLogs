using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Services;

internal class PersonalChatMessageService(IPersonalChatMessageRepository repository, IGenericRepository<PersonalChat, PersonalChatId> chatRepository, IMapper mapper) 
    : IPersonalChatMessageService
{
    private readonly IPersonalChatMessageRepository _repository = repository;
    private readonly IGenericRepository<PersonalChat, PersonalChatId> _chatRepository = chatRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PersonalChatMessageDto> CreateAsync(PersonalChatMessageDto createMessage)
    {
        var chat = await _chatRepository.GetByIdAsync(createMessage.PersonalChatId)
                            ?? throw new PersonalChatNotFoundException(createMessage.PersonalChatId);

        var personalChatMessage = new PersonalChatMessage(createMessage.Username, createMessage.Message, createMessage.Time, chat.Id, createMessage.AppUserId);

        var createdMessage = await _repository.CreateAsync(personalChatMessage);

        return createdMessage.ToDTO(_mapper);
    }

    public async Task UpdateChatMessageAsync(PersonalChatMessageId id,
                                         string? message,
                                         MessageStatus? status,
                                         MessageMarkedType? markedType)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (status.HasValue)
        {
            entity.UpdateStatus(status.Value);
        }

        if (markedType.HasValue)
        {
            entity.UpdateMarker(markedType.Value);
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            entity.EditMessage(message);
        }

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<PersonalChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new PersonalChatMessageNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int page = 1, int pageSize = 100)
    {
        var result = await _repository.GetByChatIdAsync(chatId, page, pageSize);

        return result.ToDTOCollection(_mapper);
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _repository.CountByChatIdAsync(chatId);

        return count;
    }
}

