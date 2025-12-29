using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class PlayerParseInfoModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int SpecId { get; set; }

    [Range(0, int.MaxValue)]
    public int ClassId { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageEfficiency { get; set; }

    [Range(0, int.MaxValue)]
    public int HealEfficiency { get; set; }

    [Range(0, int.MaxValue)]
    public int BossId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
