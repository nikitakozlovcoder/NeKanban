namespace NeKanban.ExceptionHandling;

public static class Exceptions
{
    public static string UserAlreadyAddedToDesk { get; set; } = nameof(UserAlreadyAddedToDesk);
    public static string CantRemoveOwnerFromDesk { get; set; } = nameof(CantRemoveOwnerFromDesk);
    public static string CantDeleteColumnWithThisType { get; set; } = nameof(CantDeleteColumnWithThisType);
}