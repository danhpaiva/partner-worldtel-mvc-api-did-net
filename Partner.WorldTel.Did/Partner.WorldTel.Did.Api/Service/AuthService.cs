using Microsoft.IdentityModel.Tokens;
using Partner.WorldTel.Did.Api.DTO;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Partner.WorldTel.Did.Api.Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly Dictionary<string, string> _refreshTokens = new(); // Em prod: use Redis ou DB

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await FindUserNameAsync(request.Username);
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return null;

        return await GenerateTokensAsync(user);
    }

    public async Task<LoginResponse> GenerateTokensAsync(AppUser user)
    {
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        _refreshTokens[user.Username] = refreshToken; // Simulação

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpiryMinutes")),
            Role = user.Role,
            Username = user.Username
        };
    }

    public Task<bool> ValidateRefreshTokenAsync(string refreshToken, string username)
    {
        return Task.FromResult(
            _refreshTokens.TryGetValue(username, out var storedToken) && storedToken == refreshToken
        );
    }
    public async Task<AppUser?> FindUserNameAsync(string username)
    {
        var users = new List<AppUser>
        {
            new() { Id = 1, Username = "admin", PasswordHash = HashPassword("admin123"), Role = "Admin", Email = "admin@worldtel.com" },
            new() { Id = 2, Username = "partner", PasswordHash = HashPassword("partner123"), Role = "Partner", Email = "partner@worldtel.com" }
        };

        return await Task.FromResult(users.FirstOrDefault(u => u.Username == username));
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }

    private string GenerateJwtToken(AppUser user)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("userid", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("AccessTokenExpiryMinutes")),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
