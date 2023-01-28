using NeKanban.Security.Constants;

namespace NeKanban.Logic.Models.DeskUserModels;

public class DeskUserRoleChangeModel
{
    public required RoleType Role { get; set; }
}
