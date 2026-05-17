using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobroLens.Models;

public class MeaslesScan : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ChildId { get; set; }

    [Required]
    public Guid UploadedById { get; set; }

    public string? AnalysisResultJson { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal ConfidenceScore { get; set; }

    [Column(TypeName = "decimal(18, 10)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(18, 10)")]
    public decimal? Longitude { get; set; }

    public ScanStatus Status { get; set; } = ScanStatus.Pending;
}

