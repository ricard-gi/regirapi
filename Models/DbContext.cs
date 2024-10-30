using Microsoft.EntityFrameworkCore;

namespace regirapi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la relació entre Issue i Project (N a 1)
            modelBuilder.Entity<Issue>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // Elimina les tasques quan s'elimina el projecte

            // Configura la relació entre Issue i User (N a 1)
            modelBuilder.Entity<Issue>()
                .HasOne(t => t.User)
                .WithMany(u => u.Issues)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Manté les tasques sense assignació quan s'elimina l'usuari
        }
    }
}
