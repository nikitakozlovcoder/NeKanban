using NeKanban.Common.Constants;

namespace NeKanban.Common.Entities;

public class DeskUser : IHasPk<int>
{
    public int Id { get; set; }
    public required int DeskId { get; set; }
    public required int UserId { get; set; }
    public required int? RoleId { get; set; }
    public required bool IsOwner { get; set; }
    public PreferenceType Preference { get; set; } = PreferenceType.Normal;
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual Role? Role { get; set; }
    public virtual ApplicationUser? User { get; set; }
    public virtual Desk? Desk { get; set; }
}