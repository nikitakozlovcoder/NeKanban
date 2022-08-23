using Microsoft.Extensions.DependencyInjection;
using NeKanban.Logic.Services.Columns;
using NeKanban.Logic.Services.Desks;
using NeKanban.Logic.Services.DesksUsers;
using NeKanban.Logic.Services.MyDesks;
using NeKanban.Logic.Services.ToDos;
using NeKanban.Logic.Services.ToDos.ToDoUsers;
using NeKanban.Logic.Services.Tokens;
using NeKanban.Services.MyDesks;
using NeKanban.Services.ToDos.ToDoUsers;

namespace NeKanban.Logic.Configuration;

public static class ServicesConfiguration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenProviderService, TokenProviderService>();
        services.AddScoped<IDesksService, DesksService>();
        services.AddScoped<IDeskUserService, DeskUserService>();
        services.AddScoped<IMyDesksService, MyDesksService>();
        services.AddScoped<IColumnsService, ColumnsService>();
        services.AddScoped<IToDoService, ToDoService>();
        services.AddScoped<IToDoUserService, ToDoUserService>();
    }
}