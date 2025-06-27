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
public class GroupChatMessageController(IGroupChatMessageService<GroupChatMessageDto, int> chatMessageService, IMapper mapper, ILogger<GroupChatMessageController> logger) : ControllerBase
{
    private readonly IGroupChatMessageService<GroupChatMessageDto, int> _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatMessageController> _logger = logger;

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

            var count = await _chatMessageService.CountByChatIdAsync(chatId);
            ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

            return Ok(count);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var groupChatMessages = await _chatMessageService.GetAllAsync();
            ArgumentNullException.ThrowIfNull(groupChatMessages, nameof(groupChatMessages));

            return Ok(groupChatMessages);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all group chat messages failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var groupChatMessage = await _chatMessageService.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(groupChatMessage, nameof(groupChatMessage));

            return Ok(groupChatMessage);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get group chat message by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, string groupChatUserId, int pageSize)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentNullException.ThrowIfNullOrEmpty(groupChatUserId, nameof(groupChatUserId));
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1, nameof(pageSize));

            var messages = await _chatMessageService.GetByChatIdAsync(chatId, groupChatUserId, pageSize);
            ArgumentNullException.ThrowIfNull(messages, nameof(messages));

            return Ok(messages);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get group chat messages by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, string groupChatUserId, int offset, int pageSize)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentNullException.ThrowIfNullOrEmpty(groupChatUserId, nameof(groupChatUserId));
            ArgumentOutOfRangeException.ThrowIfLessThan(offset, 0, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1, nameof(pageSize));

            var messages = await _chatMessageService.GetMoreByChatIdAsync(chatId, groupChatUserId, offset, pageSize);
            ArgumentNullException.ThrowIfNull(messages, nameof(messages));

            return Ok(messages);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get more group chat messages by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage);

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            var createdGroupChatMessage = await _chatMessageService.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdGroupChatMessage, nameof(createdGroupChatMessage));

            return Ok(createdGroupChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));
            ArgumentOutOfRangeException.ThrowIfZero(chatMessage.Id, nameof(chatMessage.Id));

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            var rowsAffected = await _chatMessageService.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update group chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var rowsAffected = await _chatMessageService.DeleteAsync(id);
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
