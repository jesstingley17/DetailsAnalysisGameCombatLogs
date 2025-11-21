using AutoMapper;
using CombatAnalysis.UserAPI.Models;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.UserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class RequestToConnectController(IService<RequestToConnectDto, int> service, IMapper mapper, ILogger<RequestToConnectController> logger) : ControllerBase
{
    private readonly IService<RequestToConnectDto, int> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RequestToConnectController> _logger = logger;

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

    [HttpGet("findByOwnerId/{id}")]
    public async Task<IActionResult> FindByOwnerId(string id)
    {
        var result = await _service.GetByParamAsync(c => c.AppUserId, id);

        return Ok(result);
    }

    [HttpGet("findByUserId/{id}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        var result = await _service.GetByParamAsync(c => c.ToAppUserId, id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RequestToConnectModel request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid RequestToConnect create received: {@RequestToConnect}", request);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<RequestToConnectDto>(request);
            var result = await _service.CreateAsync(map);

            return Ok(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create request to connect.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var entityDeleted = await _service.DeleteAsync(id);
            if (!entityDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
