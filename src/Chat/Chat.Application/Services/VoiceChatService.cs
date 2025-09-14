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

internal class VoiceChatService(IGenericRepository<VoiceChat, VoiceChatId> repository, IMapper mapper) : IVoiceChatService
{
    private readonly IGenericRepository<VoiceChat, VoiceChatId> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<VoiceChatDto> CreateAsync(VoiceChatDto createChat)
    {
        var chat = new VoiceChat(createChat.Id, createChat.AppUserId);

        var createdItem = await _repository.CreateAsync(chat);

        return createdItem.ToDTO(_mapper);
    }

    public async Task DeleteAsync(string id)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _repository.DeleteAsync(id);

        scope.Complete();
    }

    public async Task<IEnumerable<VoiceChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<VoiceChatDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id)
                 ?? throw new VoiceChatNotFoundException(id);

        return result.ToDTO(_mapper);
    }
}
