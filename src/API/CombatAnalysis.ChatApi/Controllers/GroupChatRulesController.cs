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
public class GroupChatRulesController(IService<GroupChatRulesDto, int> groupChatRulesService, IMapper mapper, ILogger<GroupChatRulesController> logger) 
    : ControllerBase
{
    private readonly IService<GroupChatRulesDto, int> _groupChatRulesService = groupChatRulesService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatRulesController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var groupChatRules = await _groupChatRulesService.GetAllAsync();
            ArgumentNullException.ThrowIfNull(groupChatRules, nameof(groupChatRules));

            return Ok(groupChatRules);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all group chat rules failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var groupChatRules = await _groupChatRulesService.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(groupChatRules, nameof(groupChatRules));

            return Ok(groupChatRules);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get group chat rules by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var groupChatRules = await _groupChatRulesService.GetByParamAsync(u => u.ChatId, id);
            ArgumentNullException.ThrowIfNull(groupChatRules, nameof(groupChatRules));

            return Ok(groupChatRules);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Find group chat rules by chat id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatRulesModel groupChatRules)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(groupChatRules, nameof(groupChatRules));

            var map = _mapper.Map<GroupChatRulesDto>(groupChatRules);
            var createdGroupChatRules = await _groupChatRulesService.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdGroupChatRules, nameof(createdGroupChatRules));

            return Ok(createdGroupChatRules);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat rules failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatRulesModel groupChatRules)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(groupChatRules, nameof(groupChatRules));

            var map = _mapper.Map<GroupChatRulesDto>(groupChatRules);
            var rowsAffected = await _groupChatRulesService.UpdateAsync(map);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update group chat rules failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var rowsAffected = await _groupChatRulesService.DeleteAsync(id);
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
