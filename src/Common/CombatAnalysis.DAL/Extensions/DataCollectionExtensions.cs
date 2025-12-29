using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Generic;
using CombatAnalysis.DAL.Repositories;
using CombatAnalysis.DAL.Repositories.Filters;
using CombatAnalysis.DAL.Repositories.StoredProcedures;
using CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;
using CombatAnalysis.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.DAL.Extensions;

public static class DataCollectionExtensions
{
    public static void CombatParserDALDependencies(this IServiceCollection services, string connectionString, int commandTimeout)
    {
        DBConfigurations.CommandTimeout = commandTimeout;

        services.AddDbContext<CombatParserContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IContextService, ContextService>();

        services.AddScoped<IGenericRepositoryBatch<PlayerParseInfo>, SPPlayerParseInfoRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<SpecializationScore>, SPSpecializationScoreRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<PlayerDeath>, SPPlayerDeathRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<DamageDoneGeneral>, SPDamageDoneGeneralRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<HealDoneGeneral>, SPHealDoneGeneralRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<DamageTakenGeneral>, SPDamageTakenGeneralRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<ResourceRecoveryGeneral>, SPResourceRecoveryGeneralRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<CombatAura>, SPCombatAuraRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<CombatPlayerPosition>, SPCombatPlayerPositionRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<DamageDone>, SPDamageDoneRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<HealDone>, SPHealDoneRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<DamageTaken>, SPDamageTakenRepositoryBatch>();
        services.AddScoped<IGenericRepositoryBatch<ResourceRecovery>, SPResourceRecoveryRepositoryBatch>();

        services.AddScoped<IBossRepository, BossRepository>();
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

        services.AddScoped(typeof(IGenericRepository<>), typeof(SPGenericRepository<>));
    }
}
