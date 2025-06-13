using CombatAnalysis.ChatDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Data;

public class ChatSQLContext(DbContextOptions<ChatSQLContext> options) : DbContext(options)
{

    public DbSet<VoiceChat>? VoiceChat { get; }

    public DbSet<PersonalChat>? PersonalChat { get; }

    public DbSet<PersonalChatMessage>? PersonalChatMessage { get; }

    public DbSet<GroupChat>? GroupChat { get; }

    public DbSet<GroupChatRules>? GroupChatRules { get; }

    public DbSet<GroupChatMessage>? GroupChatMessage { get; }

    public DbSet<UnreadGroupChatMessage>? UnreadGroupChatMessage { get; }

    public DbSet<GroupChatUser>? GroupChatUser { get; }
}
