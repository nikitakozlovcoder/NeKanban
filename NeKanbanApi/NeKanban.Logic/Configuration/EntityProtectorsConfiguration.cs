using Microsoft.Extensions.DependencyInjection;
using NeKanban.Data.Entities;
using NeKanban.Logic.EntityProtectors;

namespace NeKanban.Logic.Configuration;

public static class EntityProtectorsConfiguration
{
    public static void AddEntityProtectors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEntityProtector<Desk>, DeskEntityProtector>();
        serviceCollection.AddScoped<IEntityProtector<DeskUser>, DeskUserEntityProtector>();
        serviceCollection.AddScoped<IEntityProtector<Column>, ColumnEntityProtector>();
        serviceCollection.AddScoped<IEntityProtector<ToDo>, ToDoEntityProtector>();
        serviceCollection.AddScoped<IEntityProtector<ToDoUser>, ToDoUserProtector>();
    }
}