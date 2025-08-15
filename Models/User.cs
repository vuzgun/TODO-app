using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
        
        // Navigation property
        public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
