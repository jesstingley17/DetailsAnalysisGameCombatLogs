using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;

namespace Chat.Application.Services;

internal class GroupChatRulesService(IGroupChatRepository repository, IMapper mapper) : IGroupChatRulesService
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatRulesDto> CreateAsync(GroupChatRulesDto createRules)
    {
        var rules = await _repository.AddRulesAsync(createRules.ToEntity(_mapper))
                 ?? throw new GroupChatRulesNotFoundException(0);

        return rules.ToDTO(_mapper);
    }

    public async Task DeleteAsync(int chatId)
    {
        await _repository.RemoveRulesAsync(chatId);
    }

    public async Task<GroupChatRulesDto> GetByChatIdAsync(int chatId)
    {
        var chat = await _repository.GetByIdAsync(chatId)
                 ?? throw new GroupChatNotFoundException(chatId);

        var rules = chat.Rules
                ?? throw new GroupChatRulesNotFoundException(0);

        return rules.ToDTO(_mapper);
    }

    public async Task UpdateAsync(GroupChatRulesDto updateRules)
    {
        await _repository.UpdateRulesAsync(updateRules.ToEntity(_mapper));
    }
}
