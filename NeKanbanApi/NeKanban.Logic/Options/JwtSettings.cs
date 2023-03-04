namespace NeKanban.Logic.Options;

public class JwtSettings
{
    public required string RefreshSecret { get; set; }
    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int Minutes { get; set; }
    public required int RefreshMinutes { get; set; }
    public required int ClockSkewMinutes { get; set; }
}