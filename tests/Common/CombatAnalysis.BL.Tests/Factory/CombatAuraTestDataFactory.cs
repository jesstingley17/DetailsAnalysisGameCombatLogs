using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatAuraTestDataFactory
{
    public static CombatAura Create(int id = 1, string name = "Aura")
    {
        var entity = new CombatAura
        {
            Id = id,
            Name = name,
            Creator = "Solinx",
            Target = "Player",
            AuraCreatorType = 0,
            AuraType = 0,
            StartTime = TimeSpan.Parse("00:01:02"),
            FinishTime = TimeSpan.Parse("00:01:13"),
            Stacks = 1,
            CombatId = 1
        };

        return entity;
    }

    public static CombatAuraDto CreateDto(int id = 1, string name = "Aura")
    {
        var entityDto = new CombatAuraDto
        {
            Id = id,
            Name = name,
            Creator = "Solinx",
            Target = "Player",
            AuraCreatorType = 0,
            AuraType = 0,
            StartTime = TimeSpan.Parse("00:01:02"),
            FinishTime = TimeSpan.Parse("00:01:13"),
            Stacks = 1,
            CombatId = 1
        };

        return entityDto;
    }

    public static List<CombatAura> CreateCollection()
    {
        var collection = new List<CombatAura>
        {
            new () {
                Id = 1,
                Name = "Aura",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 1,
                CombatId = 1
            },
            new () {
                Id = 2,
                Name = "Aura 1",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 2,
                CombatId = 1
            },
            new () {
                Id = 3,
                Name = "Aura 2",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 3,
                CombatId = 1
            }
        };

        return collection;
    }

    public static List<CombatAuraDto> CreateDtoColelction()
    {
        var collection = new List<CombatAuraDto>
        {
            new () {
                Id = 1,
                Name = "Aura",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 1,
                CombatId = 1
            },
            new () {
                Id = 2,
                Name = "Aura 1",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 2,
                CombatId = 1
            },
            new () {
                Id = 3,
                Name = "Aura 2",
                Creator = "Solinx",
                Target = "Player",
                AuraCreatorType = 0,
                AuraType = 0,
                StartTime = TimeSpan.Parse("00:01:02"),
                FinishTime = TimeSpan.Parse("00:01:13"),
                Stacks = 3,
                CombatId = 1
            }
        };

        return collection;
    }
}
