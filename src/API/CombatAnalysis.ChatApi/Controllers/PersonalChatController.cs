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
[AllowAnonymous]
public class PersonalChatController(IService<PersonalChatDto, int> chatService, IMapper mapper, ILogger<PersonalChatController> logger) : ControllerBase
{
    private readonly IService<PersonalChatDto, int> _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PersonalChatController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var personalChat = await _chatService.GetByIdAsync(id);

            return Ok(personalChat);
        }
        catch (PersonalChatNotFoundException ex)
        {
            _logger.LogWarning("Get personal chat {Id} failed. Personal chat not found.", ex.PersonalChatId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get personal chat {Id} failed. Something wrong during extracting personal chat.", id);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatModel personalChat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PersonalChat create received: {@PersonalChat}", personalChat);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<PersonalChatDto>(personalChat);
            var createdPersonalChat = await _chatService.CreateAsync(map);

            return Ok(createdPersonalChat);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create personal chat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] PersonalChatPatch chat)
    {
        try
        {
            if (id != chat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<PersonalChatDto>(chat);
            await _chatService.UpdateAsync(map);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update personal chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update personal chat {Id} failed. Something wrong during updaring personal chat.", id);

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
            _logger.LogWarning("Delete personal chat {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Delete personal chat {Id} failed. Something wrong during deleting personal chat.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
