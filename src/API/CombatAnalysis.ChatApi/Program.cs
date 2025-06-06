using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Mapping;
using CombatAnalysis.ChatApi.Services;
using CombatAnalysis.ChatBL.Extensions;
using CombatAnalysis.ChatBL.Mapping;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

var connectionString = databasePropsOptions.Name == nameof(DatabaseType.MSSQL)
    ? databasePropsOptions.DefaultConnection
    : databasePropsOptions.FirebaseConnection;
builder.Services.ChatBLDependencies(databasePropsOptions.Name, connectionString);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ChatMapper());
    mc.AddProfile(new ChatBLMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var authenticationOptions = new Authentication();
builder.Configuration.Bind("Authentication", authenticationOptions);
var authenticationClientOptions = new AuthenticationClient();
builder.Configuration.Bind("Authentication:Client", authenticationClientOptions);
var apiOptions = new API();
builder.Configuration.Bind("API", apiOptions);

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
        builder.RequireClaim("scope", authenticationClientOptions.Scope);
    });
});

builder.Services.AddSingleton<IKafkaProducerService<string, string>, KafkaProducerService<string, string>>();
builder.Services.AddHostedService<PersonalChatMessageCountConsumerService>();
builder.Services.AddHostedService<GroupChatMessageCountConsumerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chat API",
        Version = "v1",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{apiOptions.Identity}connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { authenticationClientOptions.Scope, "Request API #1" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    },
                },
                new[] { authenticationClientOptions.Scope }
            }
        });
});



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(authenticationClientOptions.WebClientId);
    options.OAuthScopes(authenticationClientOptions.Scope);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();