using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Models.Containers;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatController(IService<GroupChatDto, int> chatService, IMapper mapper, ILogger<GroupChatController> logger, 
    IKafkaProducerService<string, string> kafkaProducer) : ControllerBase
{
    private readonly IService<GroupChatDto, int> _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatController> _logger = logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var groupChats = await _chatService.GetAllAsync();
            ArgumentNullException.ThrowIfNull(groupChats, nameof(groupChats));

            return Ok(groupChats);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get all group chats failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(id, nameof(id));

            var groupChat = await _chatService.GetByIdAsync(id);
            ArgumentNullException.ThrowIfNull(groupChat, nameof(groupChat));

            return Ok(groupChat);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            return BadRequest();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Get group chat by id failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatContainerModel container)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));

            var chatMap = _mapper.Map<GroupChatDto>(container.GroupChat);
            var createdGroupChat = await _chatService.CreateAsync(chatMap);
            ArgumentNullException.ThrowIfNull(createdGroupChat, nameof(createdGroupChat));

            var chatAction = JsonSerializer.Serialize(new GroupChatAction
            {
                ChatId = createdGroupChat.Id,
                Rules = container.GroupChatRules,
                User = container.GroupChatUser,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChat, createdGroupChat.Id.ToString(), chatAction);

            return Ok(createdGroupChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatModel chat)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chat, nameof(chat));

            ArgumentOutOfRangeException.ThrowIfZero(chat.Id, nameof(chat.Id));

            var chatMap = _mapper.Map<GroupChatDto>(chat);
            var rowsAffected = await _chatService.UpdateAsync(chatMap);
            ArgumentOutOfRangeException.ThrowIfZero(rowsAffected, nameof(rowsAffected));

            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Update group chat failed: Parameter '{ParamName}' was null.", ex.ParamName);

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

            var rowsAffected = await _chatService.DeleteAsync(id);
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
