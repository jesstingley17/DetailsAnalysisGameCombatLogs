using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatRulesController(IGroupChatRulesService chatRulesService, IMapper mapper, ILogger<GroupChatRulesController> logger) 
    : ControllerBase
{
    private readonly IGroupChatRulesService _chatRulesService = chatRulesService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatRulesController> _logger = logger;

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
            var createdGroupChatRules = await _chatRulesService.CreateAsync(map);

            return Ok(createdGroupChatRules);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Create group chat rules for chat {Id} failed: Group chat not found.", ex.GroupChatId);

            return NotFound();
        }
        catch (GroupChatRulesNotFoundException)
        {
            _logger.LogWarning("Create group chat rules failed.");

            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat rules.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{chatId:int:min(1)}")]
    public async Task<IActionResult> Update(int chatId, [FromBody] GroupChatRulesModel groupChatRules)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatRules update request received: {@GroupChatRules}", groupChatRules);

                return ValidationProblem(ModelState);
            }

            if (chatId != groupChatRules.GroupChatId)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<GroupChatRulesDto>(groupChatRules);
            await _chatRulesService.UpdateAsync(map);

            return Ok();
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Update group chat rules for chat {Id} failed. Group chat not found.", ex.GroupChatId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update group chat rules for chat {Id} failed. Group chat rules not found or modified.", chatId);

            return NotFound();
        }
    }

    [HttpDelete("{chatId:int:min(1)}")]
    public async Task<IActionResult> Delete(int chatId)
    {
        try
        {
            await _chatRulesService.DeleteAsync(chatId);

            return NoContent();
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Delete group chat rules for chat {Id} failed. Group chat not found.", ex.GroupChatId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Delete group chat rules for chat {Id} failed. Group chat rules not found or modified.", chatId);

            return NotFound();
        }
    }
}
