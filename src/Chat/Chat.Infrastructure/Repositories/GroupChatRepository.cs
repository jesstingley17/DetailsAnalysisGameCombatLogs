using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatRepository(ChatContext context) : IGroupChatRepository
{
    protected readonly ChatContext _context = context;

    public async Task<GroupChat?> CreateAsync(GroupChat item)
    {
        var entityEntry = await _context.GroupChat.AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task UpdateAsync(GroupChat item)
    {
        _context.GroupChat.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(GroupChat item)
    {
        _context.GroupChat.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GroupChat>> GetAllAsync()
    {
        var result = await _context.GroupChat.AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<GroupChat?> GetByIdAsync(int id)
    {
        var entity = await  _context.GroupChat.FirstOrDefaultAsync(g => g.Id == id);
        return entity;
    }

    public async Task<IEnumerable<GroupChatMessage>> GetChatMessagesAsync(int chatId, int page, int pageSize)
    {
        var messages = await _context.GroupChatMessage
                            .Where(m => m.GroupChatId == chatId)
                            .OrderBy(m => m.Time)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return messages;
    }
}
