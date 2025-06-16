using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Helpers;
using CombatAnalysis.Hubs.Hubs;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Kafka;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();

builder.Services.Configure<Cluster>(builder.Configuration.GetSection("Cluster"));

var authenticationOptions = new Authentication();
builder.Configuration.Bind("Authentication", authenticationOptions);
var authenticationClientOptions = new AuthenticationClient();
builder.Configuration.Bind("Authentication:Client", authenticationClientOptions);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = authenticationOptions.Authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(authenticationOptions.IssuerSigningKey),
            ValidateIssuer = true,
            ValidIssuer = authenticationOptions.Issuer,
            ValidateAudience = true,
            ValidAudiences = [authenticationClientOptions.WebClientId, authenticationClientOptions.DesktopClientId],
            ClockSkew = TimeSpan.Zero
        };
        // Skip checking HTTPS (should be HTTPS in production)
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireClaim("scope", "api1");
    });
});

builder.Services.AddSingleton<IKafkaProducerService<string, string>, KafkaProducer<string, string>>();

var cors = new CORS();
builder.Configuration.Bind("Cors", cors);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(cors.WebApp)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR()
        .AddJsonProtocol();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseRouting().UseEndpoints(endpoints =>
{
    app.MapHub<PersonalChatHub>("/personalChatHub");
    app.MapHub<PersonalChatMessagesHub>("/personalChatMessagesHub");
    app.MapHub<PersonalChatUnreadMessageHub>("/personalChatUnreadMessageHub");
    app.MapHub<GroupChatHub>("/groupChatHub");
    app.MapHub<GroupChatMessagesHub>("/groupChatMessagesHub");
    app.MapHub<GroupChatUnreadMessageHub>("/groupChatUnreadMessageHub");
    app.MapHub<VoiceChatHub>("/voiceChatHub");
});

app.UseHttpsRedirection();
app.Run();
