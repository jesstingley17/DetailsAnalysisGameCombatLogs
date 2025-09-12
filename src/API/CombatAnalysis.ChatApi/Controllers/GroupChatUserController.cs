using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Exceptions;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController(IServiceTransaction<GroupChatUserDto, string> chatUserService, IMapper mapper, ILogger<GroupChatUserController> logger) 
    : ControllerBase
{
    private readonly IServiceTransaction<GroupChatUserDto, string> _chatUserService = chatUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatUserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groupChatUsers = await _chatUserService.GetAllAsync();

        return Ok(groupChatUsers);
    }

    [HttpGet("{id:minlength(8)}")]
    public async Task<IActionResult> GetById(string id)
    {
        var groupChatUser = await _chatUserService.GetByIdAsync(id);
        if (groupChatUser == null)
        {
            _logger.LogWarning("Get group chat user by id failed: Group chat user with id {Id} not found.", id);
            return NotFound();
        }

        return Ok(groupChatUser);
    }

    [HttpGet("findMeInChat")]
    public async Task<IActionResult> Find([Required] [Range(1, int.MaxValue)] int chatId, [Required] string appUserId)
    {
        var groupChatUsers = await _chatUserService.GetByParamAsync(u => u.ChatId, chatId);

        var meInChat = groupChatUsers.FirstOrDefault(x => x.AppUserId == appUserId);
        if (meInChat == null)
        {
            _logger.LogWarning("Get group chat user by appUserId failed: Group chat user with appUserId {AppUserId} not found.", appUserId);
            return NotFound();
        }

        return Ok(meInChat);
    }

    [HttpGet("findUserInChat")]
    public async Task<IActionResult> FindUserInChat([Required] [Range(1, int.MaxValue)] int chatId, [Required] string appUserId)
    {
        var groupChatUsers = await _chatUserService.GetByParamAsync(u => u.ChatId, chatId);

        var groupChatUser = groupChatUsers.FirstOrDefault(x => x.AppUserId == appUserId);
        if (groupChatUser == null)
        {
            _logger.LogWarning("Get group chat user by appUserId failed: Group chat user with appUserId {AppUserId} not found.", appUserId);
            return NotFound();
        }

        return Ok(groupChatUser);
    }

    [HttpGet("findByUserId/{id:minlength(8)}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        var groupChatUsers = await _chatUserService.GetByParamAsync(u => u.AppUserId, id);

        return Ok(groupChatUsers);
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int chatId)
    {
        var groupChatUsers = await _chatUserService.GetByParamAsync(u => u.ChatId, chatId);

        return Ok(groupChatUsers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatUserModel chatUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatUser create received: {@ChatMessage}", chatUser);
                return ValidationProblem(ModelState);
            }

            chatUser.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<GroupChatUserDto>(chatUser);
            var createdGroupChatUser = await _chatUserService.CreateAsync(map);

            return Ok(createdGroupChatUser);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create chat user.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:minlength(8)}")]
    public async Task<IActionResult> Update(string id, [FromBody] GroupChatUserModel groupChatUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatUser update request received: {@GroupChatUser}", groupChatUser);
                return ValidationProblem(ModelState);
            }

            if (id != groupChatUser.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var map = _mapper.Map<GroupChatUserDto>(groupChatUser);
            await _chatUserService.UpdateAsync(map);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Update failed. Chat user {Id} not found or modified.", id);
            return NotFound();
        }
        catch (BusinessValidationException ex)
        {
            _logger.LogWarning("Business validation failed: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _chatUserService.DeleteAsync(id);

            return Ok();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Delete failed. Chat user {Id} not found or modified.", id);
            return NotFound();
        }
    }
}
