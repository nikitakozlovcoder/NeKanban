using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeKanban.Data.Entities;

namespace NeKanban.Data.Infrastructure;

public sealed class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.OpenConnection();
        Database.CloseConnection();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ToDoUser>()
            .HasOne(x => x.ToDo)
            .WithMany(x => x.ToDoUsers)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void Migrate()
    {
        Database.Migrate();
    }
    
    public DbSet<Desk>? Desk { get; set; }
    public DbSet<DeskUser>? DeskUser { get; set; }
    public DbSet<Column>? Column { get; set; }
    public DbSet<ToDoUser>? ToDoUser { get; set; }
    public DbSet<ToDo>? ToDo { get; set; }
}