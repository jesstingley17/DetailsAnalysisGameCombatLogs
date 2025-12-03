using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class PlayerDeathTestDataFactory
{
    public static PlayerDeath Create(int id = 1, string username = "Solinx")
    {
        var entity = new PlayerDeath
        {
            Id = id,
            Username = username,
            LastHitSpellOrItem = "Damage",
            LastHitValue = 3405,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1
        };

        return entity;
    }

    public static PlayerDeathDto CreateDto(int id = 1, string username = "Solinx")
    {
        var entityDto = new PlayerDeathDto
        {
            Id = id,
            Username = username,
            LastHitSpellOrItem = "Damage",
            LastHitValue = 3405,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1
        };

        return entityDto;
    }

    public static List<PlayerDeath> CreateCollection()
    {
        var collection = new List<PlayerDeath>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage 1",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage 2",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:51"),
                CombatPlayerId = 1
            }
        };

        return collection;
    }

    public static List<PlayerDeathDto> CreateDtoCollection()
    {
        var collection = new List<PlayerDeathDto>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage 1",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                LastHitSpellOrItem = "Damage 2",
                LastHitValue = 3405,
                Time = TimeSpan.Parse("00:01:51"),
                CombatPlayerId = 1
            }
        };

        return collection;
    }
}
