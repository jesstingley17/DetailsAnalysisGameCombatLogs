using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Requests;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Exceptions;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PersonalChatMessageController(IPersonalChatMessageService<PersonalChatMessageDto, int> chatMessageService, IMapper mapper, ILogger<PersonalChatMessageController> logger) 
    : ControllerBase
{
    private readonly IPersonalChatMessageService<PersonalChatMessageDto, int> _chatMessageService = chatMessageService;
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
        var personalChatMessage = await _chatMessageService.GetByIdAsync(id);
        if (personalChatMessage == null)
        {
            _logger.LogWarning("Get personal chat message by id failed: Personal chat message with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(personalChatMessage);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId([FromQuery] PersonalChatRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid PersonalChatMessageRequest received: {@Request}", request);
            return ValidationProblem(ModelState);
        }

        var personalChatMessages = await _chatMessageService.GetByChatIdAsync(request.ChatId, request.PageSize);

        return Ok(personalChatMessages);
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId([FromQuery] MorePersonalChatRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid MorePersonalChatMessageRequest received: {@Request}", request);
            return ValidationProblem(ModelState);
        }

        var personalChatMessages = await _chatMessageService.GetMoreByChatIdAsync(request.ChatId, request.Offset, request.PageSize);

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
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create chat message.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] PersonalChatMessageModel chatMessage)
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

            var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
            await _chatMessageService.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Personal chat message {Id} not found or modified.", id);
            return NotFound();
        }
        catch (BusinessValidationException ex)
        {
            _logger.LogWarning("Business validation failed: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _chatMessageService.DeleteAsync(id);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Personal chat message {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
