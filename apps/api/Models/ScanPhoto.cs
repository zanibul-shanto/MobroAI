using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorboLensAI.Models;

public class ScanPhoto : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ScanId { get; set; }

    [ForeignKey("ScanId")]
    public MeaslesScan? Scan { get; set; }

    [Required]
    public byte[] ImageData { get; set; } = [];
}

