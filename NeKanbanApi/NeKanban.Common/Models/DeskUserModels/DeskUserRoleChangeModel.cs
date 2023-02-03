using NeKanban.Security.Constants;

namespace NeKanban.Common.Models.DeskUserModels;

public class DeskUserRoleChangeModel
{
    public required RoleType Role { get; set; }
}
