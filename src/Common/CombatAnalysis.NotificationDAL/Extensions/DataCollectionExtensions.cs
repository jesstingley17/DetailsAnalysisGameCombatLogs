using CombatAnalysis.NotificationDAL.Data;
using CombatAnalysis.NotificationDAL.Entities;
using CombatAnalysis.NotificationDAL.Interfaces;
using CombatAnalysis.NotificationDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.NotificationDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NotificationContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGenericRepository<Notification, int>, GenericRepository<Notification, int>>();
    }
}