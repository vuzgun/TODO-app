using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Add unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Todo entity
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                
                entity.Property(e => e.Priority)
                    .HasDefaultValue(1);
                
                entity.Property(e => e.IsCompleted)
                    .HasDefaultValue(false);
                
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Configure relationship with User
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Todos)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
