using System.ComponentModel.DataAnnotations.Schema;
using NeKanban.Controllers.Models.ToDoModels;

namespace NeKanban.Data.Entities;

public class ToDo: IHasPk<int>
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = "";
    public string? Body { get; set; }
    
    public int ColumnId { get; set; }
    
    public virtual Column? Column { get; set; }
    
    [ForeignKey("ToDoId")]
    public virtual ICollection<ToDoUser> ToDoUsers { get; set; } = new List<ToDoUser>();
    
}