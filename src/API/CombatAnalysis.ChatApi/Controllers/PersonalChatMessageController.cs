using AutoMapper;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Models.Kafka;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
//[Authorize]
public class PersonalChatMessageController : ControllerBase
{
    private const string MessageCreatedTopic = "personal-chat";

    private readonly IService<PersonalChatDto, int> _chatService;
    private readonly IChatMessageService<PersonalChatMessageDto, int> _chatMessageService;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalChatMessageController> _logger;
    private readonly IChatTransactionService _chatTransactionService;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;

    public PersonalChatMessageController(IService<PersonalChatDto, int> chatService, IChatMessageService<PersonalChatMessageDto, int> chatMessageService, 
        IMapper mapper, ILogger<PersonalChatMessageController> logger, IChatTransactionService chatTransactionService, IKafkaProducerService<string, string> kafkaProducer)
    {
        _chatService = chatService;
        _chatMessageService = chatMessageService;
        _mapper = mapper;
        _logger = logger;
        _chatTransactionService = chatTransactionService;
        _kafkaProducer = kafkaProducer;
    }

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
        var map = _mapper.Map<IEnumerable<PersonalChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatMessageService.GetByIdAsync(id);
        var map = _mapper.Map<PersonalChatMessageModel>(result);

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
    public async Task<IActionResult> Create(PersonalChatMessageModel personalChatMessageModel)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(personalChatMessageModel);

            var map = _mapper.Map<PersonalChatMessageDto>(personalChatMessageModel);
            var createdPersonalChatMessage = await _chatMessageService.CreateAsync(map);

            var chat = await _chatService.GetByIdAsync(createdPersonalChatMessage.ChatId);

            var companionId = personalChatMessageModel.AppUserId == chat.CompanionId
                ? chat.InitiatorId
                : chat.CompanionId;

            var userCreatedEvent = JsonSerializer.Serialize(new PersonalChatMessageAction 
            {
                ChatId = personalChatMessageModel.ChatId, 
                AppUserId = personalChatMessageModel.AppUserId, 
                CompanionId = companionId,
                State = (int)KafkaActionState.Created,
                When = DateTime.UtcNow.ToString(),
            });
            await _kafkaProducer.ProduceAsync(MessageCreatedTopic, createdPersonalChatMessage.Id.ToString(), userCreatedEvent);

            return Ok(createdPersonalChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message failed: ${ex.Message}", personalChatMessageModel);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message failed: ${ex.Message}", personalChatMessageModel);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _chatMessageService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _chatMessageService.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
