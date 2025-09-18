using Chat.Application.Interfaces;
using Chat.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplication(this IServiceCollection services)
    {
        services.AddScoped<IGroupChatService, GroupChatService>();
        services.AddScoped<IGroupChatMessageService, GroupChatMessageService>();
        services.AddScoped<IGroupChatUserService, GroupChatUserService>();
        services.AddScoped<IPersonalChatService, PersonalChatService>();
        services.AddScoped<IPersonalChatMessageService, PersonalChatMessageService>();
        services.AddScoped<IVoiceChatService, VoiceChatService>();
    }
}
