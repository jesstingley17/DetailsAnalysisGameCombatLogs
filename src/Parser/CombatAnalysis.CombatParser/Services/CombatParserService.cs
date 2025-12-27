using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace CombatAnalysis.CombatParser.Services;

internal class CombatParserService(IFileManager fileManager, ILogger logger) : ICombatParserService
{
    private readonly IList<PlaceInformation> _zones = [];
    private readonly IFileManager _fileManager = fileManager;
    private readonly ILogger _logger = logger;

    private readonly TimeSpan _minCombatDuration = TimeSpan.Parse("00:00:20");

    public List<Combat> Combats { get; set; } = [];

    public List<CombatDetails> CombatDetails { get; set; } = [];

    public async Task<bool> FileCheckAsync(string combatLog)
    {
        using var reader = _fileManager.StreamReader(combatLog);
        var line = await reader.ReadLineAsync();

        var fileIsCorrect = !string.IsNullOrEmpty(line) && line.Contains(CombatLogKeyWords.CombatLogVersion);

        return fileIsCorrect;
    }

    public async Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken)
    {
        try
        {
            var newCombatFromLogs = new StringBuilder();
            var petsId = new Dictionary<string, List<string>>();
            var bossCombatStarted = false;

            Clear();

            foreach (var path in combatLogPaths)
            {
                var lines = await fileManager.ReadAllLinesAsync(path, cancellationToken);
                ProcessCombatLogLines(lines, petsId, ref bossCombatStarted, newCombatFromLogs);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);
        }
    }

    public void Clear()
    {
        Combats.Clear();
        _zones.Clear();
    }

    private void ProcessCombatLogLines(string[] lines, Dictionary<string, List<string>> petsId, ref bool bossCombatStarted, StringBuilder newCombatFromLogs)
    {
        foreach (var line in lines)
        {
            ProcessLine(line, newCombatFromLogs, ref bossCombatStarted, petsId);
        }
    }

    private void ProcessLine(string line, StringBuilder newCombatFromLogs, ref bool bossCombatStarted, Dictionary<string, List<string>> petsId)
    {
        if (line.Contains(CombatLogKeyWords.SpellSummon))
        {
            ParsePlayerCreatures(line, petsId);
        }

        if (line.Contains($"{CombatLogKeyWords.SwingDamage},") && line.Contains(CombatLogKeyWords.Pet))
        {
            ParsePlayerPets(line, petsId);
        }

        if (line.Contains(CombatLogKeyWords.ZoneChange))
        {
            ZoneName(line);
        }

        if (line.Contains(CombatLogKeyWords.EncounterStart))
        {
            bossCombatStarted = true;
            newCombatFromLogs.AppendLine(line);

            return;
        }

        if (!bossCombatStarted)
        {
            return;
        }

        if (line.Contains(CombatLogKeyWords.EncounterEnd))
        {
            bossCombatStarted = false;

            newCombatFromLogs.AppendLine(line);

            var newCombatFromLogsString = newCombatFromLogs.ToString();
            var combatInformationList = newCombatFromLogsString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

            GetCombatInformation(combatInformationList, petsId);

            newCombatFromLogs.Clear();
            petsId = [];
        }
        else
        {
            newCombatFromLogs.AppendLine(line);
        }
    }

    private static void ParsePlayerCreatures(string data, Dictionary<string, List<string>> creaturesId)
    {
        var combatLogParts = data.Split("  ")[1].Split(',');
        var playerId = combatLogParts[1].Contains(CombatLogKeyWords.Player) 
            ? combatLogParts[1]
            : string.Empty;
        var friendlyCreatureId = combatLogParts[1].Contains(CombatLogKeyWords.Creature)
            ? combatLogParts[1]
            : string.Empty;

        if (string.IsNullOrEmpty(playerId) && string.IsNullOrEmpty(friendlyCreatureId))
        {
            return;
        }

        var creatureId = combatLogParts[5];
        var friendCreaturePlayerId = creaturesId.FirstOrDefault(x => x.Value.Contains(friendlyCreatureId)).Key;
        if (!string.IsNullOrEmpty(friendCreaturePlayerId))
        {
            if (creaturesId.TryGetValue(friendCreaturePlayerId, out var petList))
            {
                petList.Add(creatureId);
            }
        }
        else
        {
            if (!creaturesId.TryGetValue(playerId, out var petList))
            {
                petList = new List<string>();
                creaturesId[playerId] = petList;
            }

            petList.Add(creatureId);
        }
    }

    private static void ParsePlayerPets(string data, Dictionary<string, List<string>> petsId)
    {
        var combatLogParts = data.Split("  ")[1].Split(',');

        if (combatLogParts[3].Contains("0x10a48"))
        {
            return;
        }

        var playerId = combatLogParts[10].Contains(CombatLogKeyWords.Player) ? combatLogParts[10] : string.Empty;

        if (string.IsNullOrEmpty(playerId))
        {
            return;
        }

        var petId = combatLogParts[1];
        if (!petsId.TryGetValue(playerId, out var petList))
        {
            petList = new List<string>();
            petsId[playerId] = petList;
        }

        if (!petList.Any(x => x.Equals(petId)))
        {
            petList.Add(petId);
        }
    }

    private void GetCombatInformation(List<string> builtCombat, Dictionary<string, List<string>> petsId)
    {
        if (!builtCombat[^1].Contains(CombatLogKeyWords.EncounterEnd))
        {
            return;
        }

        var boss = new Boss
        {
            GameId = GetGameBossId(builtCombat[0]),
            Difficult = GetDifficulty(builtCombat[0]),
            Size = GetGroupSize(builtCombat[0])
        };

        var combat = new Combat
        {
            Boss = boss,
            Data = builtCombat,
            IsWin = GetCombatResult(builtCombat[^1]),
            StartDate = GetTime(builtCombat[0]),
            FinishDate = GetTime(builtCombat[^1]),
            PetsId = petsId,
        };

        var duration = combat.FinishDate - combat.StartDate;
        if (duration < _minCombatDuration)
        {
            return;
        }

        var players = GetCombatPlayers(combat);
        combat.Players = players;

        CalculatingCommonCombatDetails(combat);

        AddNewCombat(combat);
    }

    private static int GetGameBossId(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var gameBossId = data.Split(',')[1];
        var convertToInt = Convert.ToInt32(gameBossId);

        return convertToInt;
    }

    private static int GetDifficulty(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var difficulty = data.Split(',')[3];
        var convertToInt = Convert.ToInt32(difficulty);

        return convertToInt;
    }

    private static int GetGroupSize(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var groupSize = data.Split(',')[4];
        var convertToInt = Convert.ToInt32(groupSize);

        return convertToInt;
    }

    private static bool GetCombatResult(string combatFinish)
    {
        var data = combatFinish.Split("  ");
        var split = data[1].Split(',');
        var combatResult = int.Parse(split[split.Length - 1]);
        var isWin = combatResult == 1;

        return isWin;
    }

    private static DateTimeOffset GetTime(string combatStart)
    {
        var parse = combatStart.Split("  ")[0];
        var combatDate = parse.Split(' ');
        var dateWithoutTime = combatDate[0].Split('/');
        var time = combatDate[1].Split('.')[0];
        var clearDate = $"{dateWithoutTime[0]}/{dateWithoutTime[1]}/{DateTimeOffset.UtcNow.Year} {time}";

        if (DateTimeOffset.TryParse(clearDate, CultureInfo.GetCultureInfo("en-EN"), DateTimeStyles.AssumeUniversal, out var date))
        {
            return date.UtcDateTime;
        }

        return DateTimeOffset.MinValue;
    }

    private static void CalculatingCommonCombatDetails(Combat combat)
    {
        var players = combat.Players;

        combat.DamageDone = players.Sum(player => player.DamageDone);
        combat.HealDone = players.Sum(player => player.HealDone);
        combat.DamageTaken = players.Sum(player => player.DamageTaken);
        combat.ResourcesRecovery = players.Sum(player => player.ResourcesRecovery);
    }

    private void AddNewCombat(Combat combat)
    {
        foreach (var item in _zones)
        {
            if (item.EntryDate < combat.StartDate)
            {
                combat.DungeonName = item.Name;
            }
        }

        Combats.Add(combat);
    }

    private List<CombatPlayer> GetCombatPlayers(Combat combat)
    {
        var combatInformations = combat.Data
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToList();

        var players = new List<CombatPlayer>();

        foreach (var item in combatInformations)
        {
            var combatInfoAsArray = item.Split([' ', ',']);
            var combatEquipmentsInfoToArray = item.Split(['[', ']']);

            var username = GetCombatPlayerUsernameById(combat.Data, combatInfoAsArray[4]);
            var averageItemLevel = GetAverageItemLevel(combatEquipmentsInfoToArray[1]);

            var usefullInformarion = combatInfoAsArray.Skip(4).ToArray();
            var stats = GetStats(usefullInformarion);

            var combatPlayerData = new CombatPlayer
            {
                Username = username,
                PlayerId = combatInfoAsArray[4],
                AverageItemLevel = double.Round(averageItemLevel, 2),
                Stats = stats,
            };

            players.Add(combatPlayerData);
        }

        var playersId = players.Select(x => x.PlayerId).ToList();

        var combatDetails = new CombatDetails(_logger, combat.PetsId);
        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        CombatDetails.Add(combatDetails);

        foreach (var item in players)
        {
            item.DamageDone = combatDetails.DamageDone[item.PlayerId].Sum(x => x.Value);
            item.HealDone = combatDetails.HealDone[item.PlayerId].Sum(x => x.Value);
            item.DamageTaken = combatDetails.DamageTaken[item.PlayerId].Sum(x => x.Value);
            item.ResourcesRecovery = combatDetails.ResourcesRecovery[item.PlayerId].Sum(x => x.Value);

        }

        return players;
    }

    private void ZoneName(string combatLog)
    {
        var parse = combatLog.Split("  ")[1];
        var name = parse.Split(',')[2];
        var clearName = name.Trim('"');

        var date = GetTime(combatLog);

        var zone = new PlaceInformation
        {
            Name = clearName,
            EntryDate = date
        };

        _zones.Add(zone);
    }

    private static string GetCombatPlayerUsernameById(List<string> combatInformation, string playerId)
    {
        var parsedUsername = string.Empty;
        for (var i = 1; i < combatInformation.Count; i++)
        {
            var data = combatInformation[i].Split(',');
            if (!combatInformation[i].Contains(CombatLogKeyWords.CombatantInfo)
                && playerId == data[1])
            {
                var username = data[2];
                parsedUsername = username.Trim('"');
                break;
            }
        }

        return parsedUsername;
    }

    private static double GetAverageItemLevel(string equipmentsInformation)
    {
        var splitEquipementsInformation = equipmentsInformation.Split("))");

        var ilvl = new List<int>();
        for (var i = 0; i < splitEquipementsInformation.Length - 2; i++)
        {
            var equipmentIlvlInformation = splitEquipementsInformation[i].Trim(',').Split(',')[1];
            if (int.TryParse(equipmentIlvlInformation, out var equipmentIlvl) && equipmentIlvl > 1)
            {
                ilvl.Add(equipmentIlvl);
            }
        }

        var averageILvl = ilvl.Average();
        return averageILvl;
    }

    private static PlayerStats GetStats(string[] combatInfo)
    {
        var stats = new PlayerStats
        {
            Faction = int.Parse(combatInfo[1]),
            Strength = int.Parse(combatInfo[2]),
            Agility = int.Parse(combatInfo[3]),
            Stamina = int.Parse(combatInfo[4]),
            Intelligence = int.Parse(combatInfo[5]),
            Spirit = int.Parse(combatInfo[6]),
            Dodge = int.Parse(combatInfo[7]),
            Parry = int.Parse(combatInfo[8]),
            Crit = int.Parse(combatInfo[10]),
            Haste = int.Parse(combatInfo[13]),
            Hit = int.Parse(combatInfo[16]),
            Expertise = int.Parse(combatInfo[19]),
            Armor = int.Parse(combatInfo[22]),
        };

        var segment = new ArraySegment<string>(combatInfo, 25, 6);
        var talents = string.Join(',', segment);
        stats.Talents = talents;

        return stats;
    }
}
