using Partner.WorldTel.Did.Api.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Partner.WorldTel.Did.Api.Models;


public class InternationalDid
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    //Example("WT-INT-550011223344")
    public string DidId { get; set; } = string.Empty;

    [Required]
    [MaxLength(4)]
    //Example("55")
    public string CountryCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(5)]
    [RegularExpression(@"^\d{1,5}$")]
    //Example("31")
    public string AreaCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [RegularExpression(@"^\d{4,10}$")]
    //Example("22334455")
    public string LocalNumber { get; set; } = string.Empty;

    [Required]
    //Example("Active")
    public DidStatus Status { get; set; } = DidStatus.Active;

    //Example("2025-11-15T16:35:00Z")
    public DateTime? ActivationDate { get; set; }

    [Required]
    [MaxLength(20)]
    //[ScalarExample("Monthly")]
    public string BillingCycle { get; set; } = "Monthly";

    [Required]
    [MaxLength(3)]
    [RegularExpression(@"^[A-Z]{3}$")]
    //Example("BRL")
    public string Currency { get; set; } = "BRL";

    [Column(TypeName = "decimal(10,2)")]
    //Example(49.90)
    public decimal MonthlyFee { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}