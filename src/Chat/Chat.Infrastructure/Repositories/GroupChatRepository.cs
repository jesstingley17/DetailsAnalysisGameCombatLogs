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
        var entity = await _context.Set<GroupChat>()
            .SingleOrDefaultAsync(g => g.Id == chatId)
                        ?? throw new EntityNotFoundException(typeof(GroupChat), chatId);

        entity.UpdateName(newName);

        await _context.SaveChangesAsync();
    }

    public async Task PassOwnerAsync(int chatId, UserId ownerId)
    {
        var entity = await _context.Set<GroupChat>()
            .SingleOrDefaultAsync(g => g.Id == chatId)
                        ?? throw new EntityNotFoundException(typeof(GroupChat), chatId);

        entity.PassOwner(ownerId);

        await _context.SaveChangesAsync();
    }

    public async Task<GroupChatRules?> AddRulesAsync(GroupChatRules rules)
    {
        var entity = await _context.GroupChat
            .SingleOrDefaultAsync(g => g.Id == rules.GroupChatId)
                ?? throw new GroupChatNotFoundException(rules.GroupChatId);

        entity.AddRules(entity.Id);

        await _context.SaveChangesAsync();

        return entity.Rules;
    }

    public async Task RemoveRulesAsync(int chatId)
    {
        var entity = await _context.GroupChat
            .SingleOrDefaultAsync(g => g.Id == chatId)
                ?? throw new GroupChatNotFoundException(chatId);

        entity.RemoveRules();

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRulesAsync(GroupChatRules updateRules)
    {
        var entity = await _context.GroupChat
            .SingleOrDefaultAsync(g => g.Id == updateRules.GroupChatId)
                ?? throw new GroupChatNotFoundException(updateRules.GroupChatId);

        entity.UpdateRules(updateRules.InvitePeople, updateRules.RemovePeople, updateRules.PinMessage, updateRules.Announcements);

        await _context.SaveChangesAsync();
    }
}
