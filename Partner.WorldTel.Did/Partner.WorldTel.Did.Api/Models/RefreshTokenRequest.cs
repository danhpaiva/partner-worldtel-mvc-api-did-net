namespace Partner.WorldTel.Did.Api.Models;

public record RefreshTokenRequest
{
    public string Username { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
