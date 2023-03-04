namespace NeKanban.Common.DTOs.RefreshTokens;

public class RefreshTokenDto
{
    public required string Token { get; set; }
    public required Guid UniqId { get; set; }
    public required DateTime ExpiresAt { get; set; }
}