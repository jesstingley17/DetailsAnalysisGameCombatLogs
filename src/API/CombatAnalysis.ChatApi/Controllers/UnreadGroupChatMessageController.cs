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
public class UnreadGroupChatMessageController(IService<UnreadGroupChatMessageDto, int> service, IMapper mapper, ILogger<UnreadGroupChatMessageController> logger) : ControllerBase
{
    private readonly IService<UnreadGroupChatMessageDto, int> _service = service;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UnreadGroupChatMessageController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var unreadGroupChatMessages = await _service.GetAllAsync();
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessages, nameof(unreadGroupChatMessages));

            return Ok(unreadGroupChatMessages);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all unread group chat messages failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var unreadGroupChatMessage = await _service.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessage, nameof(unreadGroupChatMessage));

            return Ok(unreadGroupChatMessage);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get unread group chat message by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findByMessageId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByMessageId(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var unreadGroupChatMessages = await _service.GetByParamAsync(nameof(UnreadGroupChatMessageModel.GroupChatMessageId), id);
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessages, nameof(unreadGroupChatMessages));

            return Ok(unreadGroupChatMessages);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get unread group chat message by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(UnreadGroupChatMessageModel unreadGroupChatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessage, nameof(unreadGroupChatMessage));

            var map = _mapper.Map<UnreadGroupChatMessageDto>(unreadGroupChatMessage);
            var createdUnreadGroupChatMessage = await _service.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdUnreadGroupChatMessage, nameof(createdUnreadGroupChatMessage));

            return Ok(createdUnreadGroupChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create unread group chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UnreadGroupChatMessageModel unreadGroupChatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessage, nameof(unreadGroupChatMessage));
            ArgumentOutOfRangeException.ThrowIfZero(unreadGroupChatMessage.Id, nameof(unreadGroupChatMessage.Id));

            var map = _mapper.Map<UnreadGroupChatMessageDto>(unreadGroupChatMessage);
            var rowsAffected = await _service.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update unread group chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

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

            var rowsAffected = await _service.DeleteAsync(id);
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
