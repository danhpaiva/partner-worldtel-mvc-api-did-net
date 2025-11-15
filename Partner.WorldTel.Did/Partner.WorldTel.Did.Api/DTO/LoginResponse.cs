namespace Partner.WorldTel.Did.Api.DTO;

public record LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
