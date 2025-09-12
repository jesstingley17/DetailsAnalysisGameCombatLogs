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
public class GroupChatMessageController(IGroupChatMessageService<GroupChatMessageDto, int> chatMessageService, IMapper mapper, ILogger<GroupChatMessageController> logger) : ControllerBase
{
    private readonly IGroupChatMessageService<GroupChatMessageDto, int> _chatMessageService = chatMessageService;
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
        var groupChatMessage = await _chatMessageService.GetByIdAsync(id);
        if (groupChatMessage == null)
        {
            _logger.LogWarning("Get group chat message by id failed: Group chat message with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(groupChatMessage);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId([FromQuery] GroupChatMessageRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid GroupChatMessageRequest received: {@Request}", request);
            return ValidationProblem(ModelState);
        }

        var messages = await _chatMessageService.GetByChatIdAsync(request.ChatId, request.GroupChatUserId, request.PageSize);

        return Ok(messages);
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId([FromQuery] MoreGroupChatMessageRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid MoreGroupChatMessageRequest received: {@Request}", request);
            return ValidationProblem(ModelState);
        }

        var messages = await _chatMessageService.GetMoreByChatIdAsync(request.ChatId, request.GroupChatUserId, request.Offset, request.PageSize);

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
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create chat message.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] GroupChatMessageModel chatMessage)
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

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            await _chatMessageService.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Group chat message {Id} not found or modified.", id);
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

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Group chat message {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
