using CombatAnalysis.DAL.Consts;
using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
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

        services.AddScoped<IGenericRepositoryBatch<CombatPlayerDeath>, SPPlayerDeathRepositoryBatch>();
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
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ICountRepository<DamageDone>, CountRepository<DamageDone>>();
        services.AddScoped<IGeneralFilterRepository<DamageDone>, GeneralFilterRepositroy<DamageDone>>();
        services.AddScoped<IPlayerInfoRepository<DamageDoneGeneral>, PlayerInfoRepository<DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageDone>, PlayerInfoRepository<DamageDone>>();
        services.AddScoped<ICountRepository<HealDone>, CountRepository<HealDone>>();
        services.AddScoped<IGeneralFilterRepository<HealDone>, GeneralFilterRepositroy<HealDone>>();
        services.AddScoped<IPlayerInfoRepository<HealDoneGeneral>, PlayerInfoRepository<HealDoneGeneral>>();
        services.AddScoped<IPlayerInfoRepository<HealDone>, PlayerInfoRepository<HealDone>>();
        services.AddScoped<ICountRepository<DamageTaken>, CountRepository<DamageTaken>>();
        services.AddScoped<IGeneralFilterRepository<DamageTaken>, GeneralFilterRepositroy<DamageTaken>>();
        services.AddScoped<IPlayerInfoRepository<DamageTakenGeneral>, PlayerInfoRepository<DamageTakenGeneral>>();
        services.AddScoped<IPlayerInfoRepository<DamageTaken>, PlayerInfoRepository<DamageTaken>>();
        services.AddScoped<ICountRepository<ResourceRecovery>, CountRepository<ResourceRecovery>>();
        services.AddScoped<IGeneralFilterRepository<ResourceRecovery>, GeneralFilterRepositroy<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecoveryGeneral>, PlayerInfoRepository<ResourceRecoveryGeneral>>();
        services.AddScoped<IPlayerInfoRepository<ResourceRecovery>, PlayerInfoRepository<ResourceRecovery>>();
        services.AddScoped<IPlayerInfoRepository<CombatPlayerDeath>, PlayerInfoRepository<CombatPlayerDeath>>();

        services.AddScoped<ICombatPlayerRepository, CombatPlayerRepository>();

        services.AddScoped<IDamageFilterRepository, DamageFilterRepository>();

        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<ISpecializationScoreRepository, SpecializationScoreRepository>();
        services.AddScoped<IBestSpecializationScoreRepository, BestSpecializationScoreRepository>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}
