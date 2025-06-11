using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatMessageController(IChatMessageService<GroupChatMessageDto, int> chatMessageService, IMapper mapper, ILogger<GroupChatMessageController> logger,
    IKafkaProducerService<string, string> kafkaProducer) : ControllerBase
{
    private readonly IChatMessageService<GroupChatMessageDto, int> _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatMessageController> _logger = logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var count = await _chatMessageService.CountByChatIdAsync(chatId);

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatMessageService.GetAllAsync();
        var map = _mapper.Map<IEnumerable<GroupChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatMessageService.GetByIdAsync(id);
        var map = _mapper.Map<GroupChatMessageModel>(result);

        return Ok(map);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        var messages = await _chatMessageService.GetByChatIdAsync(chatId, pageSize);

        return Ok(messages);
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        var messages = await _chatMessageService.GetMoreByChatIdAsync(chatId, offset, pageSize);

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage);

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            var createdGroupChatMessage = await _chatMessageService.CreateAsync(map);

            var chatAction = JsonSerializer.Serialize(new GroupChatMessageAction
            {
                ChatId = chatMessage.ChatId,
                GroupChatUserId = chatMessage.GroupChatUserId,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, createdGroupChatMessage.Id.ToString(), chatAction);

            return Ok(createdGroupChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", chatMessage);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", chatMessage);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _chatMessageService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var affectedRows = await _chatMessageService.DeleteAsync(id);

        return Ok(affectedRows);
    }
}
