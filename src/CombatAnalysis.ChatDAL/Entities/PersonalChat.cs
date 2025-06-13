using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatAnalysis.ChatDAL.Entities;

public class PersonalChat
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("AppUser")]
    public string InitiatorId { get; set; }

    public int InitiatorUnreadMessages { get; set; }

    [ForeignKey("AppUser")]
    public string CompanionId { get; set; }

    public int CompanionUnreadMessages { get; set; }
}
