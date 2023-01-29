using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using NeKanban.Common.Interfaces;
using NeKanban.Common.Models.ToDoModels;

namespace NeKanban.Common.Entities;

public class ToDo: IHasPk<int>, IMapSrcDest<ToDoCreateModel, ToDo>, IMapSrcDest<ToDoUpdateModel, ToDo>
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = "";
    public string? Body { get; set; }
    public int ColumnId { get; set; }
    public virtual Column? Column { get; set; }
    
    [ForeignKey("ToDoId")]
    public virtual ICollection<ToDoUser> ToDoUsers { get; set; } = new List<ToDoUser>();
    public static IMappingExpression<ToDoCreateModel, ToDo> ConfigureMap(IMappingExpression<ToDoCreateModel, ToDo> cfg)
    {
        return cfg;
    }

    public static IMappingExpression<ToDoUpdateModel, ToDo> ConfigureMap(IMappingExpression<ToDoUpdateModel, ToDo> cfg)
    {
        return cfg;
    }
}