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
public class GroupChatController(IService<GroupChatDto, int> chatService, IMapper mapper, ILogger<GroupChatController> logger) : ControllerBase
{
    private readonly IService<GroupChatDto, int> _chatService = chatService;
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
            _logger.LogWarning("Get group chat {Id} failed: Group chat not found.", ex.GroupChatId);

            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatModel chatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatModel create received: {@ChatMessage}", chatMessage);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<GroupChatDto>(chatMessage);
            var createdGroupChat = await _chatService.CreateAsync(map);

            return Ok(createdGroupChat);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] GroupChatModel chat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChat update request received: {@Chat}", chat);

                return ValidationProblem(ModelState);
            }

            if (id != chat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var chatMap = _mapper.Map<GroupChatDto>(chat);
            await _chatService.UpdateAsync(chatMap);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat {Id} failed. Group chat not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update group chat {Id} failed. Something wrong during update group chat.", id);

            return NotFound();
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
            _logger.LogWarning("Delete group chat {Id} failed. Group chat not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Delete group chat {Id} failed. Something wrong during delete group chat.", id);

            return NotFound();
        }
    }
}
