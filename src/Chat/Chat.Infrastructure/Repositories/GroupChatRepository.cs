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
    public async Task UpdateAsync(GroupChat updated)
    {
        var groupChat = await GetByIdAsync(updated.Id)
                    ?? throw new EntityNotFoundException(typeof(GroupChat), updated.Id);

        groupChat.ApplyUpdates(updated);

        await _context.SaveChangesAsync();
    }

    public async Task<GroupChatRules?> AddRulesAsync(GroupChatRules rules)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == rules.GroupChatId)
                ?? throw new GroupChatNotFoundException(rules.GroupChatId);

        chat.AddRules(chat.Id);

        _context.SaveChanges();

        return chat.Rules;
    }

    public async Task RemoveRulesAsync(int chatId)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == chatId)
                ?? throw new GroupChatNotFoundException(chatId);

        chat.RemoveRules();

        _context.SaveChanges();
    }

    public async Task UpdateRulesAsync(GroupChatRules updateRules)
    {
        var chat = await _context.GroupChat.FirstOrDefaultAsync(g => g.Id == updateRules.GroupChatId)
                ?? throw new GroupChatNotFoundException(updateRules.GroupChatId);

        chat.UpdateRules(updateRules);

        _context.SaveChanges();
    }
}
