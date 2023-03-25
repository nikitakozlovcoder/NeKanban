using Batteries.Injection.Attributes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure.QueryFilters;

namespace NeKanban.Data.Infrastructure;

[UsedImplicitly]
[Injectable<DbContext>]
public sealed class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    private readonly QueryFilterSettings _filterSettings;
    public ApplicationContext(DbContextOptions<ApplicationContext> options, QueryFilterSettings filterSettings)
        : base(options)
    {
        _filterSettings = filterSettings;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ToDo>().HasQueryFilter(x => !_filterSettings.SettingsDefinitions.ToDoDraftFilter || !x.IsDraft);
        modelBuilder.Entity<ToDoUser>().HasQueryFilter(x => !_filterSettings.SettingsDefinitions.ToDoDraftFilter || !x.ToDo!.IsDraft);
        modelBuilder.Entity<Comment>().HasQueryFilter(x => !_filterSettings.SettingsDefinitions.CommentDraftFilter || !x.IsDraft);
        modelBuilder.Entity<ToDoUser>()
            .HasOne(x => x.ToDo)
            .WithMany(x => x.ToDoUsers)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DeskUser>().HasMany(x => x.Comments)
            .WithOne(x => x.DeskUser).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<DeskUser>().HasOne(x => x.Role)
            .WithMany().OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolePermission>().HasIndex(x => new {x.RoleId, x.Permission}).IsUnique();
        modelBuilder.Entity<ToDo>().HasIndex(x => new {x.Id, x.Code}).IsUnique();
    }

    public void Migrate()
    {
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
    public DbSet<Role>? AppRoles  { get; set; }
    public DbSet<RolePermission>? RolePermissions  { get; set; }
}