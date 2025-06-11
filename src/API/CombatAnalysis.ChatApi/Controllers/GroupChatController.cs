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
public class GroupChatController(IService<GroupChatDto, int> chatService, IMapper mapper, IChatTransactionService chatTransactionService,
    ILogger<GroupChatController> logger, IKafkaProducerService<string, string> kafkaProducer) : ControllerBase
{
    private readonly IService<GroupChatDto, int> _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatController> _logger = logger;
    private readonly IChatTransactionService _chatTransactionService = chatTransactionService;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _chatService.GetAllAsync();

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Get all Group Chats failed: ${ex.Message}");

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while getting all Group Chats: ${ex.Message}");

            return BadRequest();
        }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _chatService.GetByIdAsync(id);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Get Group Chat by id failed: ${ex.Message}", id);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while getting Group Chat by id: ${ex.Message}", id);

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatContainerModel container)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(container);

            await _chatTransactionService.BeginTransactionAsync();

            var chatMap = _mapper.Map<GroupChatDto>(container.GroupChat);
            var createdGroupChat = await _chatService.CreateAsync(chatMap);

            var chatAction = JsonSerializer.Serialize(new GroupChatAction
            {
                ChatId = createdGroupChat.Id,
                Rules = container.GroupChatRules,
                User = container.GroupChatUser,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChat, createdGroupChat.Id.ToString(), chatAction);

            await _chatTransactionService.CommitTransactionAsync();

            return Ok(createdGroupChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat failed: ${ex.Message}", container);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while create Group Chat: ${ex.Message}", container);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatModel chat)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chat);

            var chatMap = _mapper.Map<GroupChatDto>(chat);
            var result = await _chatService.UpdateAsync(chatMap);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat failed: ${ex.Message}", chat);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while update Group Chat: ${ex.Message}", chat);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _chatService.DeleteAsync(id);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Remove Group Chat failed: ${ex.Message}", id);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while remove Group Chat: ${ex.Message}", id);

            return BadRequest();
        }
    }
}
