using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    [Authorize] // Require authentication for all API endpoints
    public class TodoApiController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoApiController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo - Used by frontend to load todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetTodos()
        {
            // Get user ID from claims
            var userId = GetCurrentUserId();
            var todos = await _todoService.GetAllTodosByUserIdAsync(userId);
            return Ok(todos);
        }

        // PATCH: api/todo/5/complete - Used by frontend to complete todos
        [HttpPatch("{id}/complete")]
        public async Task<ActionResult<TodoResponseDto>> MarkAsCompleted(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.MarkTodoAsCompletedAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // DELETE: api/todo/5 - Used by frontend to delete todos
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _todoService.DeleteTodoAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Helper method to get current user ID from claims
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            
            // Fallback to session if claims not available
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            return sessionUserId ?? 0;
        }
    }
}
