using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos
                .Where(t => t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _context.Todos
                .Where(t => t.IsActive)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Todo?> GetByIdAndUserIdAsync(int id, int userId)
        {
            return await _context.Todos
                .Where(t => t.Id == id && t.UserId == userId && t.IsActive)
                .Include(t => t.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo?> UpdateAsync(int id, Todo todoUpdate)
        {
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null || !existingTodo.IsActive)
                return null;

            // Update properties
            existingTodo.Title = todoUpdate.Title;
            existingTodo.Description = todoUpdate.Description;
            existingTodo.Priority = todoUpdate.Priority;
            existingTodo.IsCompleted = todoUpdate.IsCompleted;
            
            // Handle completion date
            if (todoUpdate.IsCompleted && !existingTodo.IsCompleted)
            {
                existingTodo.CompletedAt = DateTime.UtcNow;
            }
            else if (!todoUpdate.IsCompleted)
            {
                existingTodo.CompletedAt = null;
            }

            await _context.SaveChangesAsync();
            return existingTodo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null || !todo.IsActive)
                return false;

            // Soft delete - set IsActive to false
            todo.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Todo?> MarkAsCompletedAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null || !todo.IsActive)
                return null;

            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<IEnumerable<Todo>> GetByPriorityAsync(int priority)
        {
            return await _context.Todos
                .Where(t => t.Priority == priority && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetByPriorityAndUserIdAsync(int priority, int userId)
        {
            return await _context.Todos
                .Where(t => t.Priority == priority && t.UserId == userId && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetCompletedAsync()
        {
            return await _context.Todos
                .Where(t => t.IsCompleted && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CompletedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetCompletedByUserIdAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.IsCompleted && t.UserId == userId && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CompletedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetPendingAsync()
        {
            return await _context.Todos
                .Where(t => !t.IsCompleted && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetPendingByUserIdAsync(int userId)
        {
            return await _context.Todos
                .Where(t => !t.IsCompleted && t.UserId == userId && t.IsActive)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
