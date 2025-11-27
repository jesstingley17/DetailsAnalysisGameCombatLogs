using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using CombatAnalysis.CommunicationDAL.Repositories;
using CombatAnalysis.CommunicationDAL.Repositories.StoredProcedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CommunicationDAL.Extensions;

public static class DataCollectionExtensions
{
    public static void RegisterDependenciesForDAL(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CommunicationContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<IGenericRepository<CommunityDiscussion, int>, GenericRepository<CommunityDiscussion, int>>();
        services.AddScoped<IGenericRepository<CommunityDiscussionComment, int>, GenericRepository<CommunityDiscussionComment, int>>();
        services.AddScoped<IGenericRepository<CommunityUser, string>, GenericRepository<CommunityUser, string>>();
        services.AddScoped<IGenericRepository<InviteToCommunity, int>, GenericRepository<InviteToCommunity, int>>();
        services.AddScoped<ICommunityPostRepository, SPCommunityPostRepository>();
        services.AddScoped<IGenericRepository<CommunityPostComment, int>, GenericRepository<CommunityPostComment, int>>();
        services.AddScoped<IGenericRepository<CommunityPostLike, int>, GenericRepository<CommunityPostLike, int>>();
        services.AddScoped<IGenericRepository<CommunityPostDislike, int>, GenericRepository<CommunityPostDislike, int>>();
        services.AddScoped<IUserPostRepository, SPUserPostRepository>();
        services.AddScoped<IGenericRepository<UserPostComment, int>, GenericRepository<UserPostComment, int>>();
        services.AddScoped<IGenericRepository<UserPostLike, int>, GenericRepository<UserPostLike, int>>();
        services.AddScoped<IGenericRepository<UserPostDislike, int>, GenericRepository<UserPostDislike, int>>();
    }
}
