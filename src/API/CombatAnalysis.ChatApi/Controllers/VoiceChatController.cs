using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class VoiceChatController(IService<VoiceChatDto, string> service, IMapper mapper, ILogger<VoiceChatController> logger)
    : ControllerBase
{
    private readonly IService<VoiceChatDto, string> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<VoiceChatController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var voiceChats = await _service.GetAllAsync();

        return Ok(voiceChats);
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById([Required] string id)
    {
        var voiceChat = await _service.GetByIdAsync(id);
        if (voiceChat == null)
        {
            _logger.LogWarning("Get voice chat by id failed: Voice chat with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(voiceChat);
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

    [HttpPut("{id:minlength(8)}")]
    public async Task<IActionResult> Update(string id, [FromBody] VoiceChatModel voiceChat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid VoiceChat update received: {@VoiceChat}", voiceChat);
                return ValidationProblem(ModelState);
            }

            if (id != voiceChat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<VoiceChatDto>(voiceChat);
            await _service.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Voice chat {Id} not found or modified.", id);
            return NotFound();
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Voice chat {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
