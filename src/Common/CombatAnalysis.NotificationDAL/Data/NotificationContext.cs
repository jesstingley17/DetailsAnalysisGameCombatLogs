using CombatAnalysis.NotificationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.NotificationDAL.Data;

public class NotificationContext(DbContextOptions<NotificationContext> options) : DbContext(options)
{
    public DbSet<Notification>? Notification { get; }
}