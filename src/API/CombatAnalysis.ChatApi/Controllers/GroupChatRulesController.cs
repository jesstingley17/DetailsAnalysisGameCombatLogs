using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatRulesController(IService<GroupChatRulesDto, int> groupChatRulesService, IMapper mapper, ILogger<GroupChatRulesController> logger) 
    : ControllerBase
{
    private readonly IService<GroupChatRulesDto, int> _groupChatRulesService = groupChatRulesService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatRulesController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groupChatRules = await _groupChatRulesService.GetAllAsync();

        return Ok(groupChatRules);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var groupChatRules = await _groupChatRulesService.GetByIdAsync(id);
        if (groupChatRules == null)
        {
            _logger.LogWarning("Get group chat rules by id failed: Group chat rules with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(groupChatRules);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        var groupChatRules = await _groupChatRulesService.GetByParamAsync(u => u.ChatId, id);

        return Ok(groupChatRules);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatRulesModel groupChatRules)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatRules create received: {@ChatMessage}", groupChatRules);
                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<GroupChatRulesDto>(groupChatRules);
            var createdGroupChatRules = await _groupChatRulesService.CreateAsync(map);

            return Ok(createdGroupChatRules);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create chat rules.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] GroupChatRulesModel groupChatRules)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatRules update request received: {@GroupChatRules}", groupChatRules);
                return ValidationProblem(ModelState);
            }

            if (id != groupChatRules.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<GroupChatRulesDto>(groupChatRules);
            await _groupChatRulesService.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Chat rules {Id} not found or modified.", id);
            return NotFound();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _groupChatRulesService.DeleteAsync(id);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Chat {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
