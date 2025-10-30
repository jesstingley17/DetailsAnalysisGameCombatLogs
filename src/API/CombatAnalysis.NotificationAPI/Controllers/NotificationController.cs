using AutoMapper;
using CombatAnalysis.NotificationAPI.Models;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.NotificationAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
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

    [HttpGet("getByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetByRecipientId(string recipientId)
    {
        var recipientNotifications = await _notificationService.GetByParamAsync(n => n.RecipientId, recipientId);

        return Ok(recipientNotifications);
    }

    [HttpGet("getUnreadByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetUnreadByRecipientId(string recipientId)
    {
        var recipientNotifications = await _notificationService.GetByParamAsync(n => n.RecipientId, recipientId);
        var unreadNotifications = recipientNotifications.Where(n => n.Status == 0).ToList();

        return Ok(unreadNotifications);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationModel notification)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            var createdNotification = await _notificationService.CreateAsync(mappedNotification);

            return Ok(createdNotification);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create notification.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] NotificationModel notification)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Notification update request received: {@Notification}", notification);

                return ValidationProblem(ModelState);
            }

            if (id != notification.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            await _notificationService.UpdateAsync(mappedNotification);

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update notification.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _notificationService.DeleteAsync(id);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
