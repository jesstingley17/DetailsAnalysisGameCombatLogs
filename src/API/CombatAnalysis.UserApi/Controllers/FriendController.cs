using AutoMapper;
using Azure.Core;
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
public class FriendController(IFriendService service, IMapper mapper, ILogger<FriendController> logger) : ControllerBase
{
    private readonly IFriendService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<FriendController> _logger = logger;

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

    [HttpGet("searchByUserId/{id}")]
    public async Task<IActionResult> SearchByUserId(string id)
    {
        var forWhomId = await _service.GetByParamAsync(nameof(FriendModel.ForWhomId), id);
        var whoFriendId = await _service.GetByParamAsync(nameof(FriendModel.WhoFriendId), id);
        var friends = forWhomId.Concat(whoFriendId);

        return Ok(friends);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FriendModel friend)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Friend create received: {@Friend}", friend);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<FriendDto>(friend);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create friend.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FriendModel friend)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Friend update request received: {@Friend}", friend);

                return ValidationProblem(ModelState);
            }

            if (id != friend.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<FriendDto>(friend);
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

