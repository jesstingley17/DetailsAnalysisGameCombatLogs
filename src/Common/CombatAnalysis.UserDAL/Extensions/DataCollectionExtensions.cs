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
        services.AddDbContext<UserContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendRepository, FriendRepository>();
        services.AddScoped<IGenericRepository<Customer, string>, GenericRepository<Customer, string>>();
        services.AddScoped<IGenericRepository<BannedUser, int>, GenericRepository<BannedUser, int>>();
        services.AddScoped<IGenericRepository<RequestToConnect, int>, GenericRepository<RequestToConnect, int>>();

    }
}
