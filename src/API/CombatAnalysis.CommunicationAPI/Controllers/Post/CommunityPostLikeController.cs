using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationAPI.Controllers.Post;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CommunityPostLikeController(IService<CommunityPostLikeDto, int> service, IMapper mapper, ILogger<CommunityPostLikeController> logger) : ControllerBase
{
    private readonly IService<CommunityPostLikeDto, int> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CommunityPostLikeController> _logger = logger;

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

    [HttpGet("searchByPostId/{id:int:min(1)}")]
    public async Task<IActionResult> SearchByPostId(int id)
    {
        var result = await _service.GetByParamAsync(c => c.CommunityPostId, id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityPostLikeModel communityPostLike)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CommunityPostLike cretae request received: {@CommunityPostLike}", communityPostLike);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<CommunityPostLikeDto>(communityPostLike);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create community post like.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPostLikeModel communityPostLike)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CommunityPostLike update request received: {@CommunityPostLike}", communityPostLike);

                return ValidationProblem(ModelState);
            }

            if (id != communityPostLike.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<CommunityPostLikeDto>(communityPostLike);
            await _service.UpdateAsync(map);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update community post like.");

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
}
