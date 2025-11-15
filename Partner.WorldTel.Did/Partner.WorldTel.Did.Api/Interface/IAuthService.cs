using Partner.WorldTel.Did.Api.DTO;
using Partner.WorldTel.Did.Api.Models;

namespace Partner.WorldTel.Did.Api.Interface;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken, string username);
    Task<LoginResponse> GenerateTokensAsync(AppUser user);
}
