using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorboLensAI.Models;

public class LocationLog : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 10)")]
    public decimal Latitude { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 10)")]
    public decimal Longitude { get; set; }

    public bool WithChild { get; set; } = false;
}

