namespace CombatAnalysis.EnhancedWebApp.Server.Models;

public class LogEntry
{
    public string Level { get; set; } = "info";

    public string Message { get; set; } = "";

    public object? Context { get; set; }
}