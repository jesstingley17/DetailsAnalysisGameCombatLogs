using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplication(this IServiceCollection services)
    {
        services.AddScoped<IService<GroupChatDto, int>, GroupChatService>();
    }
}
