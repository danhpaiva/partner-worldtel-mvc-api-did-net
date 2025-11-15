namespace Partner.WorldTel.Did.Api.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // Admin, Partner, User
    public string Email { get; set; } = string.Empty;
}
