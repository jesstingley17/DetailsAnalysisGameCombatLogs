using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Details;

public class CombatDetails(ILogger logger)
{
    private readonly string[] _dieds =
    [
        CombatLogKeyWords.UnitDied,
    ];
    private readonly string[] _auras =
    [
        CombatLogKeyWords.AuraApplied,
        CombatLogKeyWords.AuraAppliedDose,
        CombatLogKeyWords.AuraRemoved,
    ];
    private readonly string[] _positions =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.SpellCastSuccess,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
    ];
    private readonly string[] _healVariations =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
    ];
    private readonly string[] _absorbVariations =
    [
        CombatLogKeyWords.SpellAbsorbed,
    ];
    private readonly string[] _damageVariations =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
    ];
    private readonly string[] _resourceVariations =
    [
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    ];

    private readonly Dictionary<string, List<string>> _petsId = [];

    public ILogger Logger { get; private set; } = logger;

    public Dictionary<string, List<CombatPlayerPosition>> Positions { get; private set; } = [];

    public Dictionary<string, List<PlayerDeath>> PlayersDeath { get; private set; } = [];

    public Dictionary<string, List<DamageDone>> DamageDone { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageDoneGeneral { get; private set; } = [];

    public Dictionary<string, List<HealDone>> HealDone { get; private set; } = [];

    public Dictionary<string, List<HealDoneGeneral>> HealDoneGeneral { get; private set; } = [];

    public Dictionary<string, List<DamageTaken>> DamageTaken { get; private set; } = [];

    public Dictionary<string, List<DamageTakenGeneral>> DamageTakenGeneral { get; private set; } = [];

    public Dictionary<string, List<ResourceRecovery>> ResourcesRecovery { get; private set; } = [];

    public Dictionary<string, List<ResourceRecoveryGeneral>> ResourcesRecoveryGeneral { get; private set; } = [];

    public Dictionary<string, List<CombatAura>> Auras { get; private set; } = [];

    public CombatDetails(ILogger logger, Dictionary<string, List<string>> petsId) : this(logger)
    {
        _petsId = petsId;
    }

    public void Calculate(List<string> playersId, List<string> combatData, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(playersId, nameof(playersId));
            ArgumentNullException.ThrowIfNull(combatData, nameof(combatData));
            ArgumentOutOfRangeException.ThrowIfZero(playersId.Count);
            ArgumentOutOfRangeException.ThrowIfZero(combatData.Count);

            PrepareCollections(playersId);

            foreach (var item in combatData)
            {
                Parse(playersId, item, combatStarted, combatFinished);
            }
        }
        catch (ArgumentNullException ex)
        {
            Logger.LogError("Some argument was null: {Param}", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Logger.LogError("Some argument out of valid range: {Param}", ex.ParamName);
        }
    }

    private void PrepareCollections(List<string> playersId)
    {
        foreach(var playerId in playersId)
        {
            Positions.TryAdd(playerId, []);
            PlayersDeath.TryAdd(playerId, []);
            Auras.TryAdd(playerId, []);

            DamageDone.TryAdd(playerId, []);
            HealDone.TryAdd(playerId, []);
            DamageTaken.TryAdd(playerId, []);
            ResourcesRecovery.TryAdd(playerId, []);
        }
    }

    private void Parse(List<string> playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasPositions && !hasDieds && !hasAuras && !hasHeal 
            && !hasDamage && !hasAbsorb && !hasResources)
        {
            return;
        }

        var clearCombatData = RemoveTime(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(playersId, combatStarted, combatFinished);

        if (hasPositions)
        {
            var (playerId, positionsInformation) = combatDetailsManager.GetPositions(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || positionsInformation != null)
            {
                if (Positions.TryGetValue(playerId, out var collection))
                {
                    collection.Add(positionsInformation);
                }
            }
        }

        if (hasDamage)
        {
            var (playerId, damageTakenInformation) = combatDetailsManager.GetDamageTaken(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageTakenInformation != null)
            {
                if (DamageTaken.TryGetValue(playerId, out var collection))
                {
                    collection.Add(damageTakenInformation);
                }
            }
        }

        if (hasDieds)
        {
            var (playerId, playerDeathInformation) = combatDetailsManager.GetPlayerDeath(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || playerDeathInformation != null)
            {
                if (PlayersDeath.TryGetValue(playerId, out var collection))
                {
                    collection.Add(playerDeathInformation);
                }
            }
        }
        else if (hasAuras)
        {
            var allPetsId = _petsId.SelectMany(x => x.Value).ToList();
            var (creatorId, buffs) = combatDetailsManager.GetAuras(clearCombatData, Auras, allPetsId);
            if (!string.IsNullOrEmpty(creatorId) || buffs != null)
            {
                if (Auras.TryGetValue(creatorId, out var collection))
                {
                    collection.Add(buffs);
                }
                else
                {
                    Auras.Add(creatorId, [buffs]);
                }
            }
        }
        else if (hasHeal)
        {
            var (playerId, healDoneInformation) = combatDetailsManager.GetHealDone(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || healDoneInformation != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(healDoneInformation);
                }
            }
        }
        else if (hasAbsorb)
        {
            var (playerId, absorbInformation) = combatDetailsManager.GetAbsorb(clearCombatData);
            if (absorbInformation != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(absorbInformation);
                }
            }
        }
        else if (hasDamage)
        {
            var (playerId, damageDoneInformation) = combatDetailsManager.GetPlayerDamageDone(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageDoneInformation != null)
            {
                if (DamageDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(damageDoneInformation);
                }
            }

            (playerId, damageDoneInformation) = combatDetailsManager.GetPetsDamageDone(clearCombatData, _petsId);
            if (!string.IsNullOrEmpty(playerId) || damageDoneInformation != null)
            {
                if (DamageDone.TryGetValue(playerId, out var colelction))
                {
                    colelction.Add(damageDoneInformation);
                }
            }
        }
        else if (hasResources)
        {
            var (playerId, energyRecoveryInformation) = combatDetailsManager.GetResourceRecovery(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || energyRecoveryInformation != null)
            {
                if (ResourcesRecovery.TryGetValue(playerId, out var collection))
                {
                    collection.Add(energyRecoveryInformation);
                }
            }
        }
    }

    private static List<string> RemoveTime(string combatData)
    {
        var log = combatData.Split("  ");
        var parse = log[1].Split(',');

        var data = new List<string>
        {
            log[0],
        };

        data.AddRange(parse);

        return data;
    }
}
