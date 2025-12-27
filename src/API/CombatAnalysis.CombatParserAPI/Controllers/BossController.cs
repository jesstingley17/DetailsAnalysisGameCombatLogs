using CombatAnalysis.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BossController(IBossService service) : ControllerBase
{
    private readonly IBossService _service = service;

    [HttpGet]
    public async Task<IActionResult> Get(int gameBossId, int difficult, int groupSize)
    {
        var boss = await _service.GetAsync(gameBossId, difficult, groupSize);

        return Ok(boss);
    }
}
