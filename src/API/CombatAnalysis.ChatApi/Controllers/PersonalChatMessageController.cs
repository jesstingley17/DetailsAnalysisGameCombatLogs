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
public class PersonalChatMessageController(IPersonalChatMessageService<PersonalChatMessageDto, int> chatMessageService, IMapper mapper, ILogger<PersonalChatMessageController> logger) 
    : ControllerBase
{
    private readonly IPersonalChatMessageService<PersonalChatMessageDto, int> _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PersonalChatMessageController> _logger = logger;

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
            var personalChatMessages = await _chatMessageService.GetAllAsync();
            ArgumentNullException.ThrowIfNull(personalChatMessages, nameof(personalChatMessages));

            return Ok(personalChatMessages);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all personal chat messages failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var personalChatMessage = await _chatMessageService.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(personalChatMessage, nameof(personalChatMessage));

            return Ok(personalChatMessage);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get personal chat message by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1, nameof(pageSize));

            var personalChatMessages = await _chatMessageService.GetByChatIdAsync(chatId, pageSize);
            ArgumentNullException.ThrowIfNull(personalChatMessages, nameof(personalChatMessages));

            return Ok(personalChatMessages);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get personal chat messages by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentOutOfRangeException.ThrowIfLessThan(offset, 0, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1, nameof(pageSize));

            var personalChatMessages = await _chatMessageService.GetMoreByChatIdAsync(chatId, offset, pageSize);
            ArgumentNullException.ThrowIfNull(personalChatMessages, nameof(personalChatMessages));

            return Ok(personalChatMessages);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get more personal chat messages by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatMessageModel chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
            var createdPersonalChatMessage = await _chatMessageService.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdPersonalChatMessage, nameof(createdPersonalChatMessage));

            return Ok(createdPersonalChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));
            ArgumentOutOfRangeException.ThrowIfZero(chatMessage.Id, nameof(chatMessage.Id));

            var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
            var rowsAffected = await _chatMessageService.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update personal chat message failed: Parameter '{ParamName}' was null.", ex.ParamName);

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
