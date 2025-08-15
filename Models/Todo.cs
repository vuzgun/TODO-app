using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("todos")]
    public class Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        [Column("description")]
        public string Description { get; set; } = string.Empty;
        
        [Column("is_completed")]
        public bool IsCompleted { get; set; } = false;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }
        
        [Column("priority")]
        public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
