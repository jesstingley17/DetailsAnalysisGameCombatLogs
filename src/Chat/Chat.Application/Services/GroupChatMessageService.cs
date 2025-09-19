using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Services;

internal class GroupChatMessageService(IGroupChatMessageRepository repository, IGenericRepository<GroupChat, GroupChatId> chatRepository, IGenericRepository<GroupChatUser, GroupChatUserId> userRepository,
    IMapper mapper) : IGroupChatMessageService
{
    private readonly IGroupChatMessageRepository _repository = repository;
    private readonly IGenericRepository<GroupChat, GroupChatId> _chatRepository = chatRepository;
    private readonly IGenericRepository<GroupChatUser, GroupChatUserId> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatMessageDto> CreateAsync(GroupChatMessageDto createMessage)
    {
        var chat = await _chatRepository.GetByIdAsync(createMessage.GroupChatId) 
                            ?? throw new GroupChatNotFoundException(createMessage.GroupChatId);

        var user = await _userRepository.GetByIdAsync(createMessage.GroupChatUserId)
                            ?? throw new GroupChatUserNotFoundException(createMessage.GroupChatUserId);

        chat.EnsureUserIsMember(user);

        var groupChatMessage = new GroupChatMessage(user.Username, createMessage.Message, chat.Id, user.Id, createMessage.Status, createMessage.Type, createMessage.MarkedType);

        var createdMessage = await _repository.CreateAsync(groupChatMessage);

        return createdMessage.ToDTO(_mapper);
    }

    public async Task ReadMessagesLessThanAsync(int chatId, int messageId)
    {
        await _repository.ReadMessagesLessThanAsync(chatId, messageId);
    }

    public async Task UpdateAsync(GroupChatMessageDto item)
    {
        await _repository.UpdateAsync(item.ToEntity(_mapper));
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<GroupChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new GroupChatMessageNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, int page = 1, int pageSize = 100)
    {
        var result = await _repository.GetByChatIdAsync(chatId, page, pageSize);

        return result.ToDTOCollection(_mapper);
    }

    public async Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId, int lastReadMessageId)
    {
        int countReadUnreadMessages;
        if (lastReadMessageId == 0)
        {
            countReadUnreadMessages = await _repository.CountReadUnreadMessagesAsync(chatId, chatMessageId);
        }
        else
        {
            countReadUnreadMessages = await _repository.CountReadUnreadMessagesAsync(chatId, chatMessageId, lastReadMessageId);
        }

        return countReadUnreadMessages;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _repository.CountByChatIdAsync(chatId);

        return count;
    }
}
