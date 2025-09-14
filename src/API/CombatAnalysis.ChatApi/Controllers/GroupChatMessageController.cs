using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Models;
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
            _logger.LogWarning("Get group chat message {Id} failed: Group chat message not found.", ex.MessageId);

            return NotFound();
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
            _logger.LogWarning("Create group chat message {Id} failed: Group chat not found.", ex.GroupChatId);

            return NotFound();
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message {Id} failed: Group chat user not found.", ex.UserId);

            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat message.");

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
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat message {Id} failed. Group chat message not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update group chat message {Id} failed. Something wrong during update group chat message.", id);

            return NotFound();
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
            _logger.LogWarning(ex, "Delete group chat message {Id} failed. Group chat message not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Delete group chat message {Id} failed. Something wrong during delete group chat message.", id);

            return NotFound();
        }
    }
}
