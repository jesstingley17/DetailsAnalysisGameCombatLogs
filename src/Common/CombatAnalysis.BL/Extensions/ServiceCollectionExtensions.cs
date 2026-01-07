using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Services.Filters;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.BL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void CombatParserBLDependencies(this IServiceCollection services, string connectionString, int commandTimeout)
    {
        services.CombatParserDALDependencies(connectionString, commandTimeout);

        services.AddScoped<ICombatTransactionService, CombatTransactionService>();

        services.AddScoped<IBossService, BossService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerInfoService<DamageDoneGeneralDto>, PlayerInfoService<DamageDoneGeneralDto, DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoService<DamageDoneDto>, PlayerInfoService<DamageDoneDto, DamageDoneGeneral>>();
        services.AddScoped<IPlayerInfoPaginationService<DamageDoneDto>, PlayerInfoPaginationService<DamageDoneDto, DamageDone>>();
        services.AddScoped<ICountService<DamageDoneDto>, CountService<DamageDoneDto, DamageDone>>();
        services.AddScoped<IGeneralFilterService<DamageDoneDto>, GeneralFilterService<DamageDoneDto, DamageDone>>();

        services.AddScoped<IPlayerInfoService<HealDoneGeneralDto>, PlayerInfoService<HealDoneGeneralDto, HealDoneGeneral>>();
        services.AddScoped<IPlayerInfoService<HealDoneDto>, PlayerInfoService<HealDoneDto, HealDone>>();
        services.AddScoped<IPlayerInfoPaginationService<HealDoneDto>, PlayerInfoPaginationService<HealDoneDto, HealDone>>();
        services.AddScoped<ICountService<HealDoneDto>, CountService<HealDoneDto, HealDone>>();
        services.AddScoped<IGeneralFilterService<HealDoneDto>, GeneralFilterService<HealDoneDto, HealDone>>();

        services.AddScoped<IPlayerInfoService<DamageTakenGeneralDto>, PlayerInfoService<DamageTakenGeneralDto, DamageTakenGeneral>>();
        services.AddScoped<IPlayerInfoService<DamageTakenDto>, PlayerInfoService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<IPlayerInfoPaginationService<DamageTakenDto>, PlayerInfoPaginationService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<ICountService<DamageTakenDto>, CountService<DamageTakenDto, DamageTaken>>();
        services.AddScoped<IGeneralFilterService<DamageTakenDto>, GeneralFilterService<DamageTakenDto, DamageTaken>>();

        services.AddScoped<IPlayerInfoService<ResourceRecoveryGeneralDto>, PlayerInfoService<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>>();
        services.AddScoped<IPlayerInfoService<ResourceRecoveryDto>, PlayerInfoService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<IPlayerInfoPaginationService<ResourceRecoveryDto>, PlayerInfoPaginationService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<ICountService<ResourceRecoveryDto>, CountService<ResourceRecoveryDto, ResourceRecovery>>();
        services.AddScoped<IGeneralFilterService<ResourceRecoveryDto>, GeneralFilterService<ResourceRecoveryDto, ResourceRecovery>>();

        services.AddScoped<ICombatPlayerService, CombatPlayerService>();

        services.AddScoped<IDamageFilterService, DamageFilterService>();

        services.AddScoped<IPlayerInfoService<CombatPlayerDeathDto>, PlayerInfoService<CombatPlayerDeathDto, CombatPlayerDeath>>();
        services.AddScoped<IPlayerInfoPaginationService<CombatPlayerDeathDto>, PlayerInfoPaginationService<CombatPlayerDeathDto, CombatPlayerDeath>>();

        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddScoped<ISpecializationScoreService, SpecializationScoreService>();
        services.AddScoped<IBestSpecializationScoreService, BestSpecializationScoreService>();

        SetMutationServices(services);
        SetQueryServices(services);
    }

    private static void SetMutationServices(IServiceCollection services)
    {
        services.AddScoped<IMutationServiceBatch<CombatPlayerDeathDto>, PlayerDeathService>();
        services.AddScoped<IMutationServiceBatch<DamageDoneGeneralDto>, DamageDoneGeneralService>();
        services.AddScoped<IMutationServiceBatch<HealDoneGeneralDto>, HealDoneGeneralService>();
        services.AddScoped<IMutationServiceBatch<DamageTakenGeneralDto>, DamageTakenGeneralService>();
        services.AddScoped<IMutationServiceBatch<ResourceRecoveryGeneralDto>, ResourceRecoveryGeneralService>();
        services.AddScoped<IMutationServiceBatch<CombatAuraDto>, CombatAuraService>();
        services.AddScoped<IMutationServiceBatch<CombatPlayerPositionDto>, CombatPlayerPositionService>();
        services.AddScoped<IMutationServiceBatch<DamageDoneDto>, DamageDoneService>();
        services.AddScoped<IMutationServiceBatch<HealDoneDto>, HealDoneService>();
        services.AddScoped<IMutationServiceBatch<DamageTakenDto>, DamageTakenService>();
        services.AddScoped<IMutationServiceBatch<ResourceRecoveryDto>, ResourceRecoveryService>();

        services.AddScoped<IMutationService<CombatPlayerStatsDto>, CombatPlayerStatsService>();
        services.AddScoped<IMutationService<CombatLogDto>, CombatLogService>();
        services.AddScoped<IMutationService<CombatDto>, CombatService>();
    }

    private static void SetQueryServices(IServiceCollection services)
    {
        services.AddScoped<IQueryService<CombatLogDto>, CombatLogService>();
        services.AddScoped<IQueryService<CombatDto>, CombatService>();
        services.AddScoped<IQueryService<CombatPlayerPositionDto>, CombatPlayerPositionService>();
        services.AddScoped<IQueryService<CombatAuraDto>, CombatAuraService>();
        services.AddScoped<IQueryService<CombatPlayerDeathDto>, PlayerDeathService>();
        services.AddScoped<IQueryService<CombatPlayerStatsDto>, CombatPlayerStatsService>();
    }
}
