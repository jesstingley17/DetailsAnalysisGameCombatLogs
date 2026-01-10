using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class PlayerDeathTestDataFactory
{
    public static CombatPlayerDeath Create(int id = 1, string username = "Solinx")
    {
        var entity = new CombatPlayerDeath
        {
            Id = id,
            Username = username,
            LastHitSpell = "Damage",
            LastHitValue = 3405,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1
        };

        return entity;
    }

    public static CombatPlayerDeathDto CreateDto(int id = 1, string username = "Solinx")
    {
        var entityDto = new CombatPlayerDeathDto
        {
            Id = id,
            Username = username,
            LastHitSpell = "Damage",
            LastHitValue = 3405,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1
        };

        return entityDto;
    }

    public static List<CombatPlayerDeath> CreateCollection()
    {
        var collection = new List<CombatPlayerDeath>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                LastHitSpell = "Damage",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                LastHitSpell = "Damage 1",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                LastHitSpell = "Damage 2",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:51"),
                CombatPlayerId = 1
            }
        };

        return collection;
    }

    public static List<CombatPlayerDeathDto> CreateDtoCollection()
    {
        var collection = new List<CombatPlayerDeathDto>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                LastHitSpell = "Damage",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                LastHitSpell = "Damage 1",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                LastHitSpell = "Damage 2",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:51"),
                CombatPlayerId = 1
            }
        };

        return collection;
    }
}
