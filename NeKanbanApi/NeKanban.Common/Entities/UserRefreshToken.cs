namespace NeKanban.Common.Entities;

public class UserRefreshToken : IHasPk<int>
{
    public int Id { get; set; }
    public int ApplicationUserId { get; set; }
    public Guid Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ApplicationUser? ApplicationUser { get; set; }
}