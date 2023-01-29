using NeKanban.Common.Entities;
using NeKanban.Common.Models.ColumnModels;
using NeKanban.Common.Models.ToDoModels;

namespace NeKanban.Logic.Mappings;

public static class FromExtensions
{
    public static void FromCreateModel(this ToDo toDo, ToDoCreateModel model)
    {
        toDo.Name = model.Name ?? "";
        toDo.Body = model.Body;
    }
    
    public static void FromUpdateModel(this ToDo toDo, ToDoUpdateModel model)
    {
        toDo.Name = model.Name ?? "";
        toDo.Body = model.Body;
    }
}