using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Core;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Patches;
using CombatAnalysis.ChatApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatMessageController(IGroupChatMessageService chatMessageService, IMapper mapper, ILogger<GroupChatMessageController> logger) : ControllerBase
{
    private readonly IGroupChatMessageService _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatMessageController> _logger = logger;

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var count = await _chatMessageService.CountByChatIdAsync(chatId);

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groupChatMessages = await _chatMessageService.GetAllAsync();

        return Ok(groupChatMessages);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var groupChatMessage = await _chatMessageService.GetByIdAsync(id);

            return Ok(groupChatMessage);
        }
        catch (GroupChatMessageNotFoundException ex)
        {
            _logger.LogWarning("Get group chat message {Id} failed. Group chat message not found.", ex.MessageId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get group chat message {Id} failed. Something wrong during extracting group chat message.", id);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId([FromQuery] GroupChatMessageRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid GroupChatMessageRequest received: {@Request}", request);

            return ValidationProblem(ModelState);
        }

        var messages = await _chatMessageService.GetByChatIdAsync(request.ChatId, request.Page, request.PageSize);

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatMessageModel chatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatMessage create received: {@ChatMessage}", chatMessage);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            var createdGroupChatMessage = await _chatMessageService.CreateAsync(map);

            return Ok(createdGroupChatMessage);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message failed. Group chat {Id} not found.", ex.GroupChatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message failed. Group chat user {Id} not found.", ex.UserId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Create group chat message failed. Something wrong during creating group chat message.");

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat message.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] GroupChatMessagePatch chatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatMessage update request received: {@ChatMessage}", chatMessage);

                return ValidationProblem(ModelState);
            }

            if (id != chatMessage.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            await _chatMessageService.UpdateChatMessageAsync(chatMessage.Id, chatMessage.Message, chatMessage.Status, chatMessage.MarkedType);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat message {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. Something wrong during updating group chat message.", id);

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
            await _chatMessageService.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Delete group chat message {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Delete group chat message {Id} failed. Something wrong during deleting group chat message.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
