using AutoMapper;
using CombatAnalysis.UserApi.Models;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class BannedUserController(IService<BannedUserDto, int> service, IMapper mapper, ILogger<BannedUserController> logger) : ControllerBase
{
    private readonly IService<BannedUserDto, int> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<BannedUserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BannedUserModel bannedUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid BannedUser create received: {@BannedUser}", bannedUser);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<BannedUserDto>(bannedUser);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create banned user.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BannedUserModel bannedUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid BannedUser update request received: {@BannedUser}", bannedUser);

                return ValidationProblem(ModelState);
            }

            if (id != bannedUser.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<BannedUserDto>(bannedUser);
            await _service.UpdateAsync(map);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
