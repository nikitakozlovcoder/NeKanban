namespace NeKanban.Common.Exceptions;

public static class Exceptions
{
    public static string UserAlreadyAddedToDesk { get; set; } = nameof(UserAlreadyAddedToDesk);
    public static string CantRemoveOwnerFromDesk { get; set; } = nameof(CantRemoveOwnerFromDesk);
    public static string CantDeleteColumnWithThisType { get; set; } = nameof(CantDeleteColumnWithThisType);
    public static string CantDeleteRoleWhenAnyUserHasThisRole { get; set; } = nameof(CantDeleteRoleWhenAnyUserHasThisRole);
    public static string CantDeleteDefaultRole { get; set; } = nameof(CantDeleteDefaultRole);
}