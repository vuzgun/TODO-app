using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync();
        Task<IEnumerable<TodoResponseDto>> GetAllTodosByUserIdAsync(int userId);
        Task<TodoResponseDto?> GetTodoByIdAsync(int id);
        Task<TodoResponseDto?> GetTodoByIdAndUserIdAsync(int id, int userId);
        Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto createTodoDto, int userId);
        Task<TodoResponseDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
        Task<bool> DeleteTodoAsync(int id);
        Task<TodoResponseDto?> MarkTodoAsCompletedAsync(int id);
        Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(int priority);
        Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAndUserIdAsync(int priority, int userId);
        Task<IEnumerable<TodoResponseDto>> GetCompletedTodosAsync();
        Task<IEnumerable<TodoResponseDto>> GetCompletedTodosByUserIdAsync(int userId);
        Task<IEnumerable<TodoResponseDto>> GetPendingTodosAsync();
        Task<IEnumerable<TodoResponseDto>> GetPendingTodosByUserIdAsync(int userId);
    }
}
