using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UnreadGroupChatMessageController(IService<UnreadGroupChatMessageDto, int> service, IMapper mapper, ILogger<UnreadGroupChatMessageController> logger) : ControllerBase
{
    private readonly IService<UnreadGroupChatMessageDto, int> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UnreadGroupChatMessageController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var unreadGroupChatMessages = await _service.GetAllAsync();

        return Ok(unreadGroupChatMessages);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var unreadGroupChatMessage = await _service.GetByIdAsync(id);
        if (unreadGroupChatMessage == null)
        {
            _logger.LogWarning("Get group chat unread messages by id failed: Group chat unread message with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(unreadGroupChatMessage);
    }

    [HttpGet("findByMessageId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByMessageId(int id)
    {
        var unreadGroupChatMessages = await _service.GetByParamAsync(u => u.GroupChatMessageId, id);

        return Ok(unreadGroupChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UnreadGroupChatMessageModel unreadGroupChatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid UnreadGroupChatMessage create received: {@UnreadGroupChatMessage}", unreadGroupChatMessage);
                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<UnreadGroupChatMessageDto>(unreadGroupChatMessage);
            var createdUnreadGroupChatMessage = await _service.CreateAsync(map);

            return Ok(createdUnreadGroupChatMessage);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create unread group chat message.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] UnreadGroupChatMessageModel unreadGroupChatMessage)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid UnreadGroupChatMessage update received: {@UnreadGroupChatMessage}", unreadGroupChatMessage);
                return ValidationProblem(ModelState);
            }

            if (id != unreadGroupChatMessage.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<UnreadGroupChatMessageDto>(unreadGroupChatMessage);
            await _service.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Group chat unread message {Id} not found or modified.", id);
            return NotFound();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Group chat unread message {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
