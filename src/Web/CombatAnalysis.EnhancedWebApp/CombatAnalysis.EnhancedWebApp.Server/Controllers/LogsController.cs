using CombatAnalysis.EnhancedWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LogsController(ILogger<LogsController> logger) : ControllerBase
{
    private readonly ILogger<LogsController> _logger = logger;

    [HttpPost]
    public IActionResult Log(LogEntry log)
    {
        switch (log.Level.ToLower())
        {
            case "warn":
                _logger.LogWarning("{Message}", log.Message);
                break;
            case "error":
                _logger.LogError("{Message}", log.Message);
                break;
            default:
                _logger.LogInformation("{Message}", log.Message);
                break;
        }

        return Ok();
    }
}
