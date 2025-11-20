using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Enums;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Generic;
using CombatAnalysis.DAL.Repositories;
using CombatAnalysis.DAL.Repositories.Filters;
using CombatAnalysis.DAL.Repositories.StoredProcedure;
using CombatAnalysis.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions;

public static class DataCollectionExtensions
{
    public static void CombatParserDALDependencies(this IServiceCollection services, string dataProcessingType, string connectionString, int commandTimeout)
    {
        DBConfigurations.CommandTimeout = commandTimeout;

        services.AddDbContext<CombatParserSQLContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IPlayerInfoRepository<Combat>, SPPlayerInfoRepository<Combat>>();
        services.AddScoped<ICountRepository<DamageDone>, CountRepository<DamageDone>>();
        services.AddScoped<IGeneralFilterRepository<DamageDone>, GeneralFilterRepositroy<DamageDone>>();
        services.AddScoped<IPlayerInfoRepository<DamageDoneGeneral>, SPPlayerInfoRepository<DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageDone>, SPPlayerInfoRepository<DamageDone>>();
        services.AddScoped<ICountRepository<HealDone>, CountRepository<HealDone>>();
        services.AddScoped<IGeneralFilterRepository<HealDone>, GeneralFilterRepositroy<HealDone>>();
        services.AddScoped<IPlayerInfoRepository<HealDoneGeneral>, SPPlayerInfoRepository<HealDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<HealDone>, SPPlayerInfoRepository<HealDone>>();
        services.AddScoped<ICountRepository<DamageTaken>, CountRepository<DamageTaken>>();
        services.AddScoped<IGeneralFilterRepository<DamageTaken>, GeneralFilterRepositroy<DamageTaken>>();
        services.AddScoped<IPlayerInfoRepository<DamageTakenGeneral>, SPPlayerInfoRepository<DamageTakenGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageTaken>, SPPlayerInfoRepository<DamageTaken>>();
        services.AddScoped<ICountRepository<ResourceRecovery>, CountRepository<ResourceRecovery>>();
        services.AddScoped<IGeneralFilterRepository<ResourceRecovery>, GeneralFilterRepositroy<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecoveryGeneral>, SPPlayerInfoRepository<ResourceRecoveryGeneral>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecovery>, SPPlayerInfoRepository<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<PlayerDeath>, SPPlayerInfoRepository<PlayerDeath>>();

        services.AddScoped<IDamageFilterRepository, DamageFilterRepository>();

        services.AddScoped<ISpecScore, SPSpecScoreRepository>();

        switch (dataProcessingType)
        {
            case nameof(DataProcessingType.Default):
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                break;
            case nameof(DataProcessingType.StoredProcedure):
                services.AddScoped(typeof(IGenericRepository<>), typeof(SPGenericRepository<>));
                break;
            default:
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                break;
        }
    }
}
