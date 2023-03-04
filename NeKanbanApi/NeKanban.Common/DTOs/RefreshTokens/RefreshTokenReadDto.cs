namespace NeKanban.Common.DTOs.RefreshTokens;

public class RefreshTokenReadDto
{
    public required Guid UniqId { get; set; }
    public required Guid UserUniqueName { get; set; }
}