using AutoMapper;
using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Mapping;
using CombatAnalysis.Core.Security;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.Services.Chat;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core;

public class App : MvxApplication
{
#if DEBUG
    private const string environment = "Development";
#else
    private const string environment = "Production";
#endif

    public IConfiguration Configuration { get; }

    public App()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<App>();

        Configuration = builder.Build();
    }

    public override void Initialize()
    {
        SecurityKeys.AESKey = Configuration["EncryptedKey"] ?? string.Empty;
        SecurityKeys.IV = Configuration["EncryptedIV"] ?? string.Empty;

        API.CombatParserApi = Configuration["API:CombatParser"] ?? string.Empty;
        API.UserApi = Configuration["API:User"] ?? string.Empty;
        API.ChatApi = Configuration["API:Chat"] ?? string.Empty;
        API.Identity = Configuration["API:Identity"] ?? string.Empty;

        Hubs.Server = Configuration["Hubs:Server"] ?? string.Empty;
        Hubs.PersonalChatAddress = Configuration["Hubs:PersonalChatAddress"] ?? string.Empty;
        Hubs.PersonalChatMessagesAddress = Configuration["Hubs:PersonalChatMessagesAddress"] ?? string.Empty;
        Hubs.PersonalChatUnreadMessageAddress = Configuration["Hubs:PersonalChatUnreadMessageAddress"] ?? string.Empty;
        Hubs.GroupChatAddress = Configuration["Hubs:GroupChatAddress"] ?? string.Empty;
        Hubs.GroupChatMessagesAddress = Configuration["Hubs:GroupChatMessagesAddress"] ?? string.Empty;
        Hubs.GroupChatUnreadMessageAddress = Configuration["Hubs:GroupChatUnreadMessageAddress"] ?? string.Empty;
        Hubs.VoiceChatAddress = Configuration["Hubs:VoiceChatAddress"] ?? string.Empty;

        Authentication.ClientId = Configuration["App:Auth:ClientId"] ?? string.Empty;
        Authentication.Scope = Configuration["App:Auth:Scope"] ?? string.Empty;
        Authentication.RedirectUri = Configuration["App:Auth:RedirectUri"] ?? string.Empty;
        Authentication.Protocol = Configuration["App:Auth:Protocol"] ?? string.Empty;
        Authentication.Listener = Configuration["App:Auth:Listener"] ?? string.Empty;

        AuthenticationGrantType.Code = Configuration["App:Auth:GrantType:Code"] ?? string.Empty;
        AuthenticationGrantType.Authorization = Configuration["App:Auth:GrantType:Authorization"] ?? string.Empty;
        AuthenticationGrantType.RefreshToken = Configuration["App:Auth:GrantType:RefreshToken"] ?? string.Empty;

        AppInformation.Version = Configuration["App:Version"] ?? string.Empty;
        if (Enum.TryParse(Configuration["App:VersionType"], out AppVersionType appVersionType))
        {
            AppInformation.VersionType = appVersionType;
        }

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CombatAnalysisMapper());
        });

        var memoryCacheOptions = new MemoryCacheOptions();
        var memoryCache = new MemoryCache(memoryCacheOptions);

        AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;

        if (Mvx.IoCProvider == null)
        {
            RegisterAppStart<BasicTemplateViewModel>();

            return;
        }

        Mvx.IoCProvider.RegisterType<IFileManager, FileManager>();
        Mvx.IoCProvider.RegisterType<ICombatParserService, CombatParserService>();
        Mvx.IoCProvider.RegisterType<ICombatParserAPIService, CombatParserAPIService>();
        Mvx.IoCProvider.RegisterType<IMapper>(() => mappingConfig.CreateMapper());
        Mvx.IoCProvider.RegisterType<ILogger>(() =>
        {
            var loggerFactory = new LoggerFactory();
            return new Logger<ILogger>(loggerFactory);
        });
        Mvx.IoCProvider.RegisterType<IHttpClientHelper, HttpClientHelper>();
        Mvx.IoCProvider.RegisterType<IIdentityService, IdentityService>();
        Mvx.IoCProvider.RegisterType<ICacheService, CacheService>();
        Mvx.IoCProvider.RegisterType<IChatHubHelper, ChatHubHelper>();
        Mvx.IoCProvider.RegisterType<IPersonalChatService, PersonalChatService>();
        Mvx.IoCProvider.RegisterType<IGroupChatService, GroupChatService>();
        Mvx.IoCProvider.RegisterType<IUserService, UserService>();

        Mvx.IoCProvider.RegisterSingleton<IMemoryCache>(memoryCache);

        RegisterAppStart<BasicTemplateViewModel>();
    }

    private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        LogError(ex, "Non-UI thread exception");
    }

    private void TaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogError(e.Exception, "Unobserved Task exception");
        e.SetObserved();
    }

    private static void LogError(Exception? ex, string context)
    {
        var logger = Mvx.IoCProvider?.Resolve<ILogger>();
        logger?.LogError($"{context}: {ex?.Message}");
    }
}
