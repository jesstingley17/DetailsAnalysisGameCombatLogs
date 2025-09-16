using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class VoiceChatController(IVoiceChatService service, IMapper mapper, ILogger<VoiceChatController> logger)
    : ControllerBase
{
    private readonly IVoiceChatService _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<VoiceChatController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var voiceChats = await _service.GetAllAsync();

        return Ok(voiceChats);
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var voiceChat = await _service.GetByIdAsync(id);

            return Ok(voiceChat);
        }
        catch (VoiceChatNotFoundException ex)
        {
            _logger.LogWarning("Get voice chat by id failed: Voice chat with id {Id} not found.", ex.VoiceChatId);

            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoiceChatModel voiceChat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid VoiceChat create received: {@VoiceChat}", voiceChat);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<VoiceChatDto>(voiceChat);
            var createdVoiceChat = await _service.CreateAsync(map);

            return Ok(createdVoiceChat);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create voice chat.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Delete voice chat {Id} failed. Voice chat not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete voice chat {Id} failed. Voice chat not found or modified.", id);
            return NotFound();
        }
    }
}
