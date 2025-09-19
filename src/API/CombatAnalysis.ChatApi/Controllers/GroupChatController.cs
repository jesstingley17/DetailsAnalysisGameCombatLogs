using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Core;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Patches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatController(IGroupChatService chatService, IMapper mapper, ILogger<GroupChatController> logger) : ControllerBase
{
    private readonly IGroupChatService _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groupChats = await _chatService.GetAllAsync();

        return Ok(groupChats);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var groupChat = await _chatService.GetByIdAsync(id);

            return Ok(groupChat);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Get group chat {Id} failed. Group chat not found.", ex.GroupChatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get group chat {Id} failed. Something wrong during extracting group chat.", id);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatModel groupChat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatModel create received: {@GroupChat}", groupChat);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<GroupChatDto>(groupChat);
            var createdGroupChat = await _chatService.CreateAsync(map);

            return Ok(createdGroupChat);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] GroupChatPatch chat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChat update request received: {@GroupChat}", chat);

                return ValidationProblem(ModelState);
            }

            if (id != chat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            await _chatService.UpdateChatAsync(chat.Id, chat.Name, chat.OwnerId);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update group chat {Id} failed. Something wrong during updating group chat.", id);

            return this.ExtractDomainCode(ex.Code);
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
            await _chatService.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Delete group chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Delete group chat {Id} failed. Something wrong during deleting group chat message.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpPut("updateRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> UpdateRules(int chatId, [FromBody] GroupChatRulesModel chatRules)
    {
        try
        {
            if (chatId != chatRules.GroupChatId)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var chatRulesDto = _mapper.Map<GroupChatRulesDto>(chatRules);
            await _chatService.UpdateRulesAsync(chatRulesDto);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", chatId, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update group chat {Id} failed. Something wrong during updating group chat message.", chatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpGet("getRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetRules(int chatId)
    {
        try
        {
            var groupChatRules = await _chatService.GetRulesAsync(chatId);

            return Ok(groupChatRules);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Get group chat rules for chat {Id} failed: Group chat not found.", chatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (GroupChatRulesNotFoundException ex)
        {
            _logger.LogWarning("Get group chat rules for chat {Id} failed: Group chat rules not found.", chatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get group chat rules for chat {Id} failed. Something wrong during extracting group chat rules.", chatId);

            return this.ExtractDomainCode(ex.Code);
        }
    }
}
