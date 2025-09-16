using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController(IGroupChatUserService chatUserService, IMapper mapper, ILogger<GroupChatUserController> logger) : ControllerBase
{
    private readonly IGroupChatUserService _chatUserService = chatUserService;
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
        try
        {
            var groupChatUser = await _chatUserService.GetByIdAsync(id);

            return Ok(groupChatUser);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Get group chat user {Id} failed: Group chat user not found.", ex.UserId);

            return NotFound();
        }
    }

    [HttpGet("findByAppUserId")]
    public async Task<IActionResult> FindByAppUserId([Required] [Range(1, int.MaxValue)] int chatId, [Required] string appUserId)
    {
        try
        {
            var groupChatUser = await _chatUserService.FindByAppUserIdAsync(chatId, appUserId);

            return Ok(groupChatUser);
        }
        catch (GroupChatUserNotFoundException)
        {
            _logger.LogInformation("Find group chat user by app user {Id} finished with result 'Not found'.", appUserId);

            return NotFound();
        }
    }

    [HttpGet("findAllByAppUserId/{appUserId:minlength(8)}")]
    public async Task<IActionResult> FindAllByAppUserId(string appUserId)
    {
        var groupChatUsers = await _chatUserService.FindAllByAppUserIdAsync(appUserId);

        return Ok(groupChatUsers);
    }

    [HttpGet("findAll/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindAll(int chatId)
    {
        var groupChatUsers = await _chatUserService.FindAllAsync(chatId);

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

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat user {Id} failed. Group chat user not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update group chat user {Id} failed. Chat user not found or modified.", id);

            return NotFound();
        }
    }

    [HttpDelete("{id:minlength(8)}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _chatUserService.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Delete group chat user {Id} failed. Group chat user not found.", ex.EntityId);

            return NotFound();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Delete group chat user {Id} failed. Group chat user not found or modified.", id);

            return NotFound();
        }
    }
}
