using AutoMapper;
using CombatAnalysis.NotificationAPI.Consts;
using CombatAnalysis.NotificationAPI.Mapping;
using CombatAnalysis.NotificationBL.Extensions;
using CombatAnalysis.NotificationBL.Mapping;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

builder.Services.NotificationBLDependencies(databasePropsOptions.DefaultConnection);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new NotificationMapper());
    mc.AddProfile(new NotificationBLMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/chatapi.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API v1");
    options.InjectStylesheet("/swagger-ui/swaggerDark.css");
    //options.OAuthClientId(authenticationClientOptions.WebClientId);
    //options.OAuthScopes(authenticationClientOptions.Scope);
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

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
