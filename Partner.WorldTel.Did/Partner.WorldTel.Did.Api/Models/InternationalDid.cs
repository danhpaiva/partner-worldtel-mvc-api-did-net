using Partner.WorldTel.Did.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Partner.WorldTel.Did.Api.Models;


public class InternationalDid
{
    /// <summary>
    /// Chave primária (auto-incremento)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// ID único do DID no formato WT-INT-{CountryCode}{AreaCode}{LocalNumber}
    /// </summary>
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^WT-INT-\d{1,4}\d{1,5}\d{4,10}$")]
    //Example("WT-INT-550011223344")
    public string DidId { get; set; } = string.Empty;

    /// <summary>
    /// Código do país (E.164)
    /// </summary>
    [Required]
    [MaxLength(4)]
    [RegularExpression(@"^\d{1,4}$")]
    //Example("55")
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Código de área
    /// </summary>
    [Required]
    [MaxLength(5)]
    [RegularExpression(@"^\d{1,5}$")]
    //Example("31")
    public string AreaCode { get; set; } = string.Empty;

    /// <summary>
    /// Número local
    /// </summary>
    [Required]
    [MaxLength(10)]
    [RegularExpression(@"^\d{4,10}$")]
    //Example("22334455")
    public string LocalNumber { get; set; } = string.Empty;

    /// <summary>
    /// Status atual do DID
    /// </summary>
    [Required]
    //Example("Active")
    public DidStatus Status { get; set; } = DidStatus.Active;

    /// <summary>
    /// Data de ativação
    /// </summary>
    //Example("2025-11-15T16:35:00Z")
    public DateTime? ActivationDate { get; set; }

    /// <summary>
    /// Ciclo de cobrança
    /// </summary>
    [Required]
    [MaxLength(20)]
    //[ScalarExample("Monthly")]
    public string BillingCycle { get; set; } = "Monthly";

    /// <summary>
    /// Moeda (ISO 4217)
    /// </ triage>
    [Required]
    [MaxLength(3)]
    [RegularExpression(@"^[A-Z]{3}$")]
    //Example("BRL")
    public string Currency { get; set; } = "BRL";

    /// <summary>
    /// Taxa mensal
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    //Example(49.90)
    public decimal MonthlyFee { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}