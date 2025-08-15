using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class CreateTodoDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Range(1, 3)]
        public int Priority { get; set; } = 1;
        
    }
    
    public class UpdateTodoDto
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        // MVC checkbox requires non-nullable bool
        public bool IsCompleted { get; set; }
        
        [Range(1, 3)]
        public int? Priority { get; set; }
    }
    
    public class TodoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Priority { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
