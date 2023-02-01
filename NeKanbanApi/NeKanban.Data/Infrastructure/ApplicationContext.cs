using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Entities;

namespace NeKanban.Data.Infrastructure;

public sealed class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
       
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
        Database.EnsureCreated();
        Database.Migrate();
    }

    public void TestConnection()
    {
        Database.OpenConnection();
        Database.CloseConnection();
    }
    
    public DbSet<Desk>? Desk { get; set; }
    public DbSet<DeskUser>? DeskUser { get; set; }
    public DbSet<Column>? Column { get; set; }
    public DbSet<ToDoUser>? ToDoUser { get; set; }
    public DbSet<ToDo>? ToDo { get; set; }
    public DbSet<Comment>? Comments { get; set; }
}