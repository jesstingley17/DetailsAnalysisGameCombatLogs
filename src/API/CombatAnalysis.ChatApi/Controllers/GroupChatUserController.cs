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
public class GroupChatUserController(IServiceTransaction<GroupChatUserDto, string> chatUserService, IMapper mapper, ILogger<GroupChatUserController> logger) 
    : ControllerBase
{
    private readonly IServiceTransaction<GroupChatUserDto, string> _chatUserService = chatUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatUserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var groupChatUsers = await _chatUserService.GetAllAsync();
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            return Ok(groupChatUsers);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all group chat users failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var groupChatUser = await _chatUserService.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            return Ok(groupChatUser);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get group chat user by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findUserInChat")]
    public async Task<IActionResult> FindUserInChat(int chatId, string appUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

            ArgumentNullException.ThrowIfNull(appUserId, nameof(appUserId));

            var groupChatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), chatId);
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            var groupChatUser = groupChatUsers.FirstOrDefault(x => x.AppUserId == appUserId);
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            return Ok(groupChatUser);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get users in group chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findByUserId/{id}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var groupChatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.AppUserId), id);
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            return Ok(groupChatUsers);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Find group chat users by user id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var groupChatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), id);
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            return Ok(groupChatUsers);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Find group chat users by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findMeInChat")]
    public async Task<IActionResult> FindMeInChat(int chatId, string appUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

            ArgumentNullException.ThrowIfNull(appUserId, nameof(appUserId));

            var groupChatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), chatId);
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            var meInChat = groupChatUsers.FirstOrDefault(x => x.AppUserId == appUserId);
            ArgumentNullException.ThrowIfNull(meInChat, nameof(meInChat));

            return Ok(meInChat);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Find me in chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatUserModel chatUser)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatUser, nameof(chatUser));

            chatUser.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<GroupChatUserDto>(chatUser);
            var createdGroupChatUser = await _chatUserService.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdGroupChatUser, nameof(createdGroupChatUser));

            return Ok(createdGroupChatUser);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat ser failed:  Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatUserModel groupChatUser)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            var map = _mapper.Map<GroupChatUserDto>(groupChatUser);
            var rowsAffected = await _chatUserService.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update group chat user failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var rowsAffected = await _chatUserService.DeleteAsync(id);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }
}
