using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatRepository(ChatContext context) : GenericRepository<GroupChat, GroupChatId>(context), IGroupChatRepository
{
    public async Task UpdateNameAsync(int chatId, string newName)
    {
        var groupChat = await GetByIdAsync(chatId)
                    ?? throw new EntityNotFoundException(typeof(GroupChat), chatId);

        groupChat.UpdateName(newName);

        await _context.SaveChangesAsync();
    }

    public async Task PassOwnerAsync(int chatId, UserId ownerId)
    {
        var groupChat = await GetByIdAsync(chatId)
                    ?? throw new EntityNotFoundException(typeof(GroupChat), chatId);

        groupChat.PassOwner(ownerId);

        await _context.SaveChangesAsync();
    }

    public async Task<GroupChatRules?> AddRulesAsync(GroupChatRules rules)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == rules.GroupChatId)
                ?? throw new GroupChatNotFoundException(rules.GroupChatId);

        chat.AddRules(chat.Id);

        await _context.SaveChangesAsync();

        return chat.Rules;
    }

    public async Task RemoveRulesAsync(int chatId)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == chatId)
                ?? throw new GroupChatNotFoundException(chatId);

        chat.RemoveRules();

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRulesAsync(GroupChatRules updateRules)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == updateRules.GroupChatId)
                ?? throw new GroupChatNotFoundException(updateRules.GroupChatId);

        chat.UpdateRules(updateRules.InvitePeople, updateRules.RemovePeople, updateRules.PinMessage, updateRules.Announcements);

        await _context.SaveChangesAsync();
    }
}
