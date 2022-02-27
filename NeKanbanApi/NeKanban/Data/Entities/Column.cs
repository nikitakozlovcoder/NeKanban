using NeKanban.Constants;
using NeKanban.Controllers;

namespace NeKanban.Data.Entities;

public class Column : IHasPk<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ColumnType Type { get; set; }
    public int Order { get; set; }
    public int DeskId { get; set; }
    public virtual Desk? Desk { get; set; }
    public virtual ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
}