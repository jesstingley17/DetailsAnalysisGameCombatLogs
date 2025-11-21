using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.UserDAL.Tests.Factory;

internal static class TestDataFactory
{
    public static List<DamageDone> CreateDamageDonColelction()
    {
        var collection = new List<DamageDone>
        {
            new () {
                Id = 1,
                Creator = "Solinx",
                Target = "Boss",
                Spell = "Test",
                IsPeriodicDamage = false,
                Time = TimeSpan.Parse("00:01:10"),
                Value = 200,
                DamageType = 0,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new () {
                Id = 2,
                Creator = "Solinx",
                Target = "Boss",
                Spell = "Check",
                IsPeriodicDamage = false,
                Time = TimeSpan.Parse("00:01:11"),
                Value = 210,
                DamageType = 0,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new () {
                Id = 3,
                Creator = "Solinx",
                Target = "Boss",
                Spell = "Check",
                IsPeriodicDamage = false,
                Time = TimeSpan.Parse("00:02:11"),
                Value = 10,
                DamageType = 0,
                IsPet = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
