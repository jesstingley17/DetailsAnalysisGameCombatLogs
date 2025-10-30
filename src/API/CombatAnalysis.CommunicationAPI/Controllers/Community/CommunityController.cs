using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationAPI.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityController(ICommunityService service, IMapper mapper, ILogger<CommunityController> logger) : ControllerBase
{
    private readonly ICommunityService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CommunityController> _logger = logger;

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
    public async Task<IActionResult> Create([FromBody] CommunityModel community)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Community create request received: {@Community}", community);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CommunityDto>(community);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create community.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityModel community)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Community update request received: {@Community}", community);

                return ValidationProblem(ModelState);
            }

            if (id != community.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<CommunityDto>(community);
            await _service.UpdateAsync(map);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update community.");

            return StatusCode(500, "Internal server error.");
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

    [HttpGet("getWithPagination")]
    public async Task<IActionResult> GetWithPaginationAsync(int pageSize)
    {
        var communitites = await _service.GetAllWithPaginationAsync(pageSize);

        return Ok(communitites);
    }

    [HttpGet("getMoreWithPagination")]
    public async Task<IActionResult> GetMoreWithPaginationAsync(int offset, int pageSize)
    {
        var communitites = await _service.GetMoreWithPaginationAsync(offset, pageSize);

        return Ok(communitites);
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _service.CountAsync();

        return Ok(count);
    }
}
