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
        var chat = await _chatService.GetByIdAsync(id);
        if (chat == null)
        {
            _logger.LogWarning("Get personal chat by id failed: Personal chat with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(chat);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatModel personalChat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PersonalChat create received: {@ChatMessage}", personalChat);
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

    [HttpPost("personalChatIsAlreadyExists")]
    public async Task<IActionResult> PersonalChatCheck([FromBody] PersonalChatModel personalChat)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid PersonalChat check received: {@ChatMessage}", personalChat);
            return ValidationProblem(ModelState);
        }

        var allData = await _chatService.GetAllAsync();
        foreach (var item in allData)
        {
            if ((item.InitiatorId == personalChat.InitiatorId && item.CompanionId == personalChat.CompanionId)
                || (item.InitiatorId == personalChat.CompanionId && item.CompanionId == personalChat.InitiatorId))
            {
                return Ok(item.Id);
            }
        }

        return NotFound();
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] PersonalChatModel chat)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid PersonalChat update request received: {@Chat}", chat);
                return ValidationProblem(ModelState);
            }

            if (id != chat.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<PersonalChatDto>(chat);
            await _chatService.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Personal chat {Id} not found or modified.", id);
            return NotFound();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _chatService.DeleteAsync(id);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Personal chat {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
