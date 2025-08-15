using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync()
        {
            var todos = await _todoRepository.GetAllAsync();
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetAllTodosByUserIdAsync(int userId)
        {
            var todos = await _todoRepository.GetAllByUserIdAsync(userId);
            return todos.Select(MapToResponseDto);
        }

        public async Task<TodoResponseDto?> GetTodoByIdAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            return todo != null ? MapToResponseDto(todo) : null;
        }

        public async Task<TodoResponseDto?> GetTodoByIdAndUserIdAsync(int id, int userId)
        {
            var todo = await _todoRepository.GetByIdAndUserIdAsync(id, userId);
            return todo != null ? MapToResponseDto(todo) : null;
        }

        public async Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto createTodoDto, int userId)
        {
            var todo = new Todo
            {
                Title = createTodoDto.Title,
                Description = createTodoDto.Description,
                Priority = createTodoDto.Priority,
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            var createdTodo = await _todoRepository.CreateAsync(todo);
            return MapToResponseDto(createdTodo);
        }

        public async Task<TodoResponseDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            var existingTodo = await _todoRepository.GetByIdAsync(id);
            if (existingTodo == null) return null;

            // Create update object with only changed properties
            var updateTodo = new Todo
            {
                Id = id,
                Title = updateTodoDto.Title ?? existingTodo.Title,
                Description = updateTodoDto.Description ?? existingTodo.Description,
                Priority = updateTodoDto.Priority ?? existingTodo.Priority,
                IsCompleted = updateTodoDto.IsCompleted,
                UserId = existingTodo.UserId,
                IsActive = existingTodo.IsActive
            };

            var updatedTodo = await _todoRepository.UpdateAsync(id, updateTodo);
            return updatedTodo != null ? MapToResponseDto(updatedTodo) : null;
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            return await _todoRepository.DeleteAsync(id);
        }

        public async Task<TodoResponseDto?> MarkTodoAsCompletedAsync(int id)
        {
            var todo = await _todoRepository.MarkAsCompletedAsync(id);
            return todo != null ? MapToResponseDto(todo) : null;
        }

        public async Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(int priority)
        {
            var todos = await _todoRepository.GetByPriorityAsync(priority);
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAndUserIdAsync(int priority, int userId)
        {
            var todos = await _todoRepository.GetByPriorityAndUserIdAsync(priority, userId);
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetCompletedTodosAsync()
        {
            var todos = await _todoRepository.GetCompletedAsync();
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetCompletedTodosByUserIdAsync(int userId)
        {
            var todos = await _todoRepository.GetCompletedByUserIdAsync(userId);
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetPendingTodosAsync()
        {
            var todos = await _todoRepository.GetPendingAsync();
            return todos.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TodoResponseDto>> GetPendingTodosByUserIdAsync(int userId)
        {
            var todos = await _todoRepository.GetPendingByUserIdAsync(userId);
            return todos.Select(MapToResponseDto);
        }

        private static TodoResponseDto MapToResponseDto(Todo todo)
        {
            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                CompletedAt = todo.CompletedAt,
                Priority = todo.Priority,
                UserId = todo.UserId,
                IsActive = todo.IsActive,
                Username = todo.User?.Username ?? string.Empty
            };
        }
    }
}
