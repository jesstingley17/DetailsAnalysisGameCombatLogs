using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserDAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.UserBL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void UserBLDependencies(this IServiceCollection services, string connectionString)
    {
        services.UserDALDependencies(connectionString);

        services.AddScoped<IUserTransactionService, UserTransactionService>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IService<RequestToConnectDto, int>, RequestToConnectService>();
        services.AddScoped<IService<BannedUserDto, int>, BannedUserService>();
        services.AddScoped<IFriendService, FriendService>();
    }
}
