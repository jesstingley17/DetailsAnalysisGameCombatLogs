using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using CombatAnalysis.UserDAL.Repositories;
using CombatAnalysis.UserDAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.UserDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void UserDALDependencies(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGenericRepository<Customer, string>, Repository<Customer, string>>();
        services.AddScoped<IGenericRepository<BannedUser, int>, Repository<BannedUser, int>>();
        services.AddScoped<IGenericRepository<RequestToConnect, int>, Repository<RequestToConnect, int>>();

        services.AddScoped<IFriendRepository, FriendRepository>();
    }
}
