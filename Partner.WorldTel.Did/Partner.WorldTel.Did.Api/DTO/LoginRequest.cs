using System.ComponentModel.DataAnnotations;

namespace Partner.WorldTel.Did.Api.DTO;

public record LoginRequest
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
