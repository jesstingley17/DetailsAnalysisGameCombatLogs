namespace CombatAnalysis.BL.DTO;

public class CombatDto
{
    public int Id { get; set; }

    public string DungeonName { get; set; } = string.Empty;

    public int EnergyRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }

    public int BossId { get; set; }

    public int CombatLogId { get; set; }
}
