using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatAPI.Core;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Patches;
using CombatAnalysis.ChatAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PersonalChatMessageController(IPersonalChatMessageService chatMessageService, IMapper mapper, ILogger<PersonalChatMessageController> logger) 
    : ControllerBase
{
    private readonly IPersonalChatMessageService _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PersonalChatMessageController> _logger = logger;

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var count = await _chatMessageService.CountByChatIdAsync(chatId);

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var personalChatMessages = await _chatMessageService.GetAllAsync();

        return Ok(personalChatMessages);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var personalChatMessage = await _chatMessageService.GetByIdAsync(id);

            return Ok(personalChatMessage);
        }
        catch (PersonalChatMessageNotFoundException ex)
        {
            _logger.LogWarning("Get personal chat message {Id} failed: Personal chat message not found.", ex.MessageId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get personal chat message {Id} failed. Something wrong during extracting personal chat message.", id);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId([FromQuery] PersonalChatRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid PersonalChatMessageRequest received: {@Request}", request);
            return ValidationProblem(ModelState);
        }

        var personalChatMessages = await _chatMessageService.GetByChatIdAsync(request.ChatId, request.Page, request.PageSize);

        return Ok(personalChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatMessageModel chatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PersonalChatMessage create received: {@ChatMessage}", chatMessage);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
            var createdPersonalChatMessage = await _chatMessageService.CreateAsync(map);

            return Ok(createdPersonalChatMessage);
        }
        catch (PersonalChatNotFoundException ex)
        {
            _logger.LogWarning("Create personal chat message {Id} failed. Personal chat not found.", ex.PersonalChatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Create personal chat message failed. Something wrong during extracting personal chat message.");

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create personal chat message.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] PersonalChatMessagePatch chatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PersonalChatMessage update request received: {@ChatMessage}", chatMessage);

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
            _logger.LogWarning("Update personal chat message {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update personal chat message {Id} failed. Something wrong during updaring personal chat message.", id);

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
            _logger.LogWarning("Delete personal chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Delete personal chat message {Id} failed. Something wrong during deleting personal chat message.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
