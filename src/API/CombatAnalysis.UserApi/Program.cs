using AutoMapper;
using CombatAnalysis.UserApi.Consts;
using CombatAnalysis.UserApi.Enums;
using CombatAnalysis.UserApi.Mapping;
using CombatAnalysis.UserBL.Extensions;
using CombatAnalysis.UserBL.Mapping;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

var connection = databasePropsOptions.Name == nameof(DatabaseType.MSSQL)
    ? databasePropsOptions.DefaultConnection
    : databasePropsOptions.FirebaseConnection;
builder.Services.UserBLDependencies(databasePropsOptions.Name, connection);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserApiMapper());
    mc.AddProfile(new CustomerBLMapper());
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
    options.AddPolicy("ApiScope", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("scope", authenticationClientOptions.Scope);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{apiOptions.Identity}connect/authorize"),
                TokenUrl = new Uri($"{apiOptions.Identity}connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { authenticationClientOptions.Scope, "Request User API Authorization" }
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
                }
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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    options.OAuthClientId(authenticationClientOptions.WebClientId);
    options.OAuthScopes(authenticationClientOptions.Scope);
    options.OAuthUsePkce();

    //options.OAuth2RedirectUrl("https://localhost:5003/swagger/oauth2-redirect.html");
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();