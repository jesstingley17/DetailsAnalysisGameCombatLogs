using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Mapping;

public class BLMapper : Profile
{
    public BLMapper()
    {
        CreateMap<PlayerDto, Player>().ReverseMap();
        CreateMap<BossDto, Boss>().ReverseMap();
        CreateMap<CombatLogDto, CombatLog>().ReverseMap();
        CreateMap<CombatDto, Combat>().ReverseMap();
        CreateMap<CombatPlayerDto, CombatPlayer>().ReverseMap();
        CreateMap<CombatAuraDto, CombatAura>().ReverseMap();
        CreateMap<CombatPlayerPositionDto, CombatPlayerPosition>().ReverseMap();
        CreateMap<SpecializationDto, Specialization>().ReverseMap();
        CreateMap<SpecializationScoreDto, SpecializationScore>().ReverseMap();
        CreateMap<BestSpecializationScoreDto, BestSpecializationScore>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneDto, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<PlayerDeathDto, PlayerDeath>().ReverseMap();
        CreateMap<PlayerStatsDto, PlayerStats>().ReverseMap();
        CreateMap<CombatTargetDto, CombatTarget>().ReverseMap();
    }
}