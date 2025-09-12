using AutoMapper;
using CombatAnalysis.ChatApi.Models;
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
        var groupChat = await _chatService.GetByIdAsync(id);
        if (groupChat == null)
        {
            _logger.LogWarning("Get group chat {Id} failed: Group chat not found.", id);
            return NotFound();
        }

        return Ok(groupChat);
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
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update group chat {Id} failed. Group chat not found or modified.", id);
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
            await _chatService.DeleteAsync(id);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete group chat {Id} failed. Group chat not found or modified.", id);
            return NotFound();
        }
    }
}
