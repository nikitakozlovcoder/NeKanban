using NeKanban.Common.Constants;
using NeKanban.Security.Constants;

namespace NeKanban.Common.Entities;

public class DeskUser : IHasPk<int>
{
    public int Id { get; set; }
    public required int DeskId { get; set; }
    public required int UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    public virtual Desk? Desk { get; set; }
    public required RoleType Role { get; set; }
    public PreferenceType Preference { get; set; } = PreferenceType.Normal;
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}