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
public class PersonalChatController(IService<PersonalChatDto, int> chatService, IMapper mapper, ILogger<PersonalChatController> logger) : ControllerBase
{
    private readonly IService<PersonalChatDto, int> _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PersonalChatController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonalChatModel personalChatModel)
    {
        try
        {
            if (personalChatModel == null)
            {
                throw new ArgumentNullException(nameof(personalChatModel));
            }

            var map = _mapper.Map<PersonalChatDto>(personalChatModel);
            var createdPersonalChat = await _chatService.CreateAsync(map);

            return Ok(createdPersonalChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Personal Chat failed: ${ex.Message}", personalChatModel);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Personal Chat failed: ${ex.Message}", personalChatModel);

            return BadRequest();
        }
    }

    [HttpPost("personalChatIsAlreadyExists")]
    public async Task<IActionResult> PersonalChatCheck(PersonalChatModel model)
    {
        var allData = await _chatService.GetAllAsync();
        foreach (var item in allData)
        {
            if ((item.InitiatorId == model.InitiatorId && item.CompanionId == model.CompanionId)
                || (item.InitiatorId == model.CompanionId && item.CompanionId == model.InitiatorId))
            {
                return Ok(item.Id);
            }
        }

        return Ok(0);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatDto>(model);
            var result = await _chatService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Personal Chat failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _chatService.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
