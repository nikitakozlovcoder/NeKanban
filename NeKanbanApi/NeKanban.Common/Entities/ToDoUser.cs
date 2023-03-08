using Batteries.Repository;
using NeKanban.Common.Constants;

namespace NeKanban.Common.Entities;

public class ToDoUser : IHasPk<int>
{
    public int Id { get; set; }
    public required int ToDoId { get; set; }
    public required int DeskUserId { get; set; }
    public virtual DeskUser? DeskUser { get; set; }
    public virtual ToDo? ToDo { get; set; }
    public required ToDoUserType ToDoUserType { get; set; }
}