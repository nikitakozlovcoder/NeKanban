using NeKanban.Constants;

namespace NeKanban.Data.Entities;

public class ToDoUser : IHasPk<int>
{
    public int Id { get; set; }
    public int ToDoId { get; set; }
    public int DeskUserId { get; set; }
    public virtual DeskUser? DeskUser { get; set; }
    public virtual ToDo? ToDo { get; set; }
    public ToDoUserType ToDoUserType { get; set; }
}