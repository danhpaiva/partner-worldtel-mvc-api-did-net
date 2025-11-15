using Microsoft.AspNetCore.Mvc;
using Partner.WorldTel.Did.Api.DTO;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Models;
using Partner.WorldTel.Did.Api.Service;

namespace Partner.WorldTel.Did.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.AuthenticateAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            var isValid = await _authService.ValidateRefreshTokenAsync(request.RefreshToken, request.Username);
            if (!isValid)
                return Unauthorized(new { message = "Refresh token inválido ou expirado." });

            var user = await ((AuthService)_authService).FindUserNameAsync(request.Username);
            if (user == null)
                return Unauthorized(new { message = "Usuário não encontrado." });

            var newTokens = await _authService.GenerateTokensAsync(user);
            return Ok(newTokens);
        }
    }
}