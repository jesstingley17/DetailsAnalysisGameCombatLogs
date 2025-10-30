using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Interfaces;
using CombatAnalysis.NotificationBL.Services;
using CombatAnalysis.NotificationDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.NotificationBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void NotificationBLDependencies(this IServiceCollection services, string connectionString)
    {
        services.RegisterDependenciesForDAL(connectionString);

        services.AddScoped<IService<NotificationDto, int>, NotificationService>();
    }
}
