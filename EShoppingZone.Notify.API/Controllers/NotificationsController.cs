using EShoppingZone.Notify.API.Domain;
using EShoppingZone.Notify.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Notify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            var notification = await _service.CreateNotificationAsync(request.UserId, request.Type, request.Title, request.Message, request.Email);
            return CreatedAtAction(nameof(GetNotifications), new { userId = request.UserId }, notification);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var notifications = await _service.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("{userId}/unread")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var count = await _service.GetUnreadCountAsync(userId);
            return Ok(new { UnreadCount = count });
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _service.MarkAsReadAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("read-all/{uid}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> MarkAllAsRead(int uid)
        {
            await _service.MarkAllAsReadAsync(uid);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _service.DeleteNotificationAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

    public class CreateNotificationRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
