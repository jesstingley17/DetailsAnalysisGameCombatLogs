namespace CombatAnalysis.Core.Models;

public class CombatModel
{
    public int Id { get; set; }

    public int Number { get; set; }

    public int UniqueCombatCount { get; set; }

    public int[] Items { get; set; } = [];

    public string DungeonName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Difficulty { get; set; }

    public List<string> Data { get; set; } = [];

    public int ResourcesRecovery { get; set; }

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public List<CombatPlayerModel> Players { get; set; } = [];

    public Dictionary<string, List<string>> PetsId { get; set; } = [];

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }

    public int BossId { get; set; }

    public int CombatLogId { get; set; }
}
