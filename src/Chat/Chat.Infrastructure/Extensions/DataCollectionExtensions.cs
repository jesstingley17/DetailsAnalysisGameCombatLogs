using Chat.Domain.Repositories;
using Chat.Infrastructure.Persistence;
using Chat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Extensions;

public static class DataCollectionExtensions
{
    public static void AddChatInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ChatContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGroupChatRepository, GroupChatRepository>();
    }
}
