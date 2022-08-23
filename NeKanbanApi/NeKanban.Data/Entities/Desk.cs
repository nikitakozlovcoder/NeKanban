using NeKanban.Data.Infrastructure;

namespace NeKanban.Data.Entities;

public class Desk : IHasPk<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? InviteLink { get; set; }
    public virtual ICollection<DeskUser> DeskUsers { get; set; } = new List<DeskUser>();
    public virtual ICollection<Column> Columns { get; set; } = new List<Column>();

}