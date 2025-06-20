using AutoMapper;
using CombatAnalysis.NotificationAPI.Models;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.NotificationAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
//[Authorize]
public class NotificationController(IService<NotificationDto, int> notificationService, IMapper mapper, ILogger<NotificationController> logger) : ControllerBase
{
    private readonly IService<NotificationDto, int> _notificationService = notificationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<NotificationController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allNotifications = await _notificationService.GetAllAsync();

        return Ok(allNotifications);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await _notificationService.GetByIdAsync(id);

        return Ok(notification);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NotificationModel notification)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            var createdNotification = await _notificationService.CreateAsync(mappedNotification);

            return Ok(createdNotification);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Notification failed: ${ex.Message}", notification);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Notification failed: ${ex.Message}", notification);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(NotificationModel notification)
    {
        try
        {
            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            var affectedRows = await _notificationService.UpdateAsync(mappedNotification);

            return Ok(affectedRows);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Notification failed: ${ex.Message}", notification);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Notification failed: ${ex.Message}", notification);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var affectedRows = await _notificationService.DeleteAsync(id);

        return Ok(affectedRows);
    }
}
