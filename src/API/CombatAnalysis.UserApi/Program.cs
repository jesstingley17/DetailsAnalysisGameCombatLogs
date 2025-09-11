using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.UserApi.Consts;
using CombatAnalysis.UserApi.Enums;
using CombatAnalysis.UserApi.Mapping;
using CombatAnalysis.UserBL.Extensions;
using CombatAnalysis.UserBL.Mapping;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

var connection = databasePropsOptions.Name == nameof(DatabaseType.MSSQL)
    ? databasePropsOptions.DefaultConnection
    : databasePropsOptions.FirebaseConnection;
builder.Services.UserBLDependencies(databasePropsOptions.Name, connection);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddExpressionMapping();
    mc.AddProfile(new UserApiMapper());
    mc.AddProfile(new UserBLMapper());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var authenticationOptions = new Authentication();
builder.Configuration.Bind("Authentication", authenticationOptions);
var authenticationClientOptions = new AuthenticationClient();
builder.Configuration.Bind("Authentication:Client", authenticationClientOptions);
var apiOptions = new API();
builder.Configuration.Bind("API", apiOptions);

var audiences = authenticationClientOptions.Audiences.Split(',');
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
                ValidAudiences = audiences,
                ClockSkew = TimeSpan.Zero
            };
            // Skip checking HTTPS (should be HTTPS in production)
            options.RequireHttpsMetadata = false;
        });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ApiScope", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireClaim("scope", authenticationClientOptions.Scopes);
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
                    { authenticationClientOptions.Scopes, "Request User API Authorization" }
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
            new[] { authenticationClientOptions.Scopes }
        }
    });
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/userapi.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    //options.OAuthClientId(authenticationClientOptions.WebClientId);
    //options.OAuthScopes(authenticationClientOptions.Scope);
    //options.OAuthUsePkce();
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers().RequireAuthorization("ApiScope");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exceptionHandlerPathFeature?.Error;

        Log.Error(ex, "Unhandled exception occurred");

        var result = new
        {
            message = "An unexpected error occurred. Please try again later."
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

app.Run();