using NeKanban.Security.Constants;

namespace NeKanban.Common.Entities;

public class RolePermission : IHasPk<int>
{
    public int Id { get; set; }
    public required int RoleId { get; set; }
    public required PermissionType Permission { get; set; }
}