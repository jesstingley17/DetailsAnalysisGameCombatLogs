using AutoMapper;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.UserBL.Extensions;
using CombatAnalysis.UserBL.Mapping;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Core;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Mapping;
using CombatAnalysisIdentity.Services;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<API>(builder.Configuration.GetSection("API"));
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AuthenticationGrantType>(builder.Configuration.GetSection("Authentication:GrantType"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

var databasePropsOptions = new DatabaseProps();
builder.Configuration.Bind("Database", databasePropsOptions);

builder.Services.RegisterIdentityDependencies(databasePropsOptions.DefaultConnection);
builder.Services.UserBLDependencies(databasePropsOptions.Name, databasePropsOptions.UserConnection);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new IdentityMapper());
    mc.AddProfile(new CustomerBLMapper());
    mc.AddProfile(new CombatAnalysisIdentityMapper());
});
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var certificateOptions = new Certificate();
builder.Configuration.Bind("Certificate", certificateOptions);

var certificate = new X509Certificate2(certificateOptions.PfxPath, certificateOptions.PWD);
builder.Services.AddIdentityServer()
            .AddSigningCredential(certificate)
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryApiScopes(Config.ApiScopes);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("default");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.MapRazorPages();

app.MapControllers();

app.Run();
