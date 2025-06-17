using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var voiceChats = await _service.GetAllAsync();
            ArgumentNullException.ThrowIfNull(voiceChats, nameof(voiceChats));

            return Ok(voiceChats);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all voice chats failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNullOrEmpty(id, nameof(id));

            var voiceChat = await _service.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(voiceChat, nameof(voiceChat));

            return Ok(voiceChat);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get voice chat by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(VoiceChatModel voiceChat)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(voiceChat, nameof(voiceChat));

            var map = _mapper.Map<VoiceChatDto>(voiceChat);
            var createdVoiceChat = await _service.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdVoiceChat, nameof(createdVoiceChat));

            return Ok(createdVoiceChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create voice chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(VoiceChatModel voiceChat)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(voiceChat, nameof(voiceChat));
            ArgumentOutOfRangeException.ThrowIfNullOrEmpty(voiceChat.Id, nameof(voiceChat.Id));

            var map = _mapper.Map<VoiceChatDto>(voiceChat);
            var rowsAffected = await _service.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update voice chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(id, nameof(id));

            var rowsAffected = await _service.DeleteAsync(id);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }
}
