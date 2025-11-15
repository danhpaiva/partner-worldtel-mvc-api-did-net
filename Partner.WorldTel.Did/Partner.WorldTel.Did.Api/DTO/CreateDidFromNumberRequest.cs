using System.ComponentModel.DataAnnotations;

namespace Partner.WorldTel.Did.Api.DTO;

public class CreateDidFromNumberRequest
{
    [Required]
    [RegularExpression(@"^\+\d{5,15}$", ErrorMessage = "Formato E.164 inválido.")]
    public string E164Number { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }
}
