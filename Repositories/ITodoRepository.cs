using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<IEnumerable<Todo>> GetAllByUserIdAsync(int userId);
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo?> GetByIdAndUserIdAsync(int id, int userId);
        Task<Todo> CreateAsync(Todo todo);
        Task<Todo?> UpdateAsync(int id, Todo todo);
        Task<bool> DeleteAsync(int id);
        Task<Todo?> MarkAsCompletedAsync(int id);
        Task<IEnumerable<Todo>> GetByPriorityAsync(int priority);
        Task<IEnumerable<Todo>> GetByPriorityAndUserIdAsync(int priority, int userId);
        Task<IEnumerable<Todo>> GetCompletedAsync();
        Task<IEnumerable<Todo>> GetCompletedByUserIdAsync(int userId);
        Task<IEnumerable<Todo>> GetPendingAsync();
        Task<IEnumerable<Todo>> GetPendingByUserIdAsync(int userId);
    }
}
