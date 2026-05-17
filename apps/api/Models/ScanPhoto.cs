using System.ComponentModel.DataAnnotations;

namespace MobroLens.Models;

public class ScanPhoto : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ScanId { get; set; }

    [Required]
    public byte[] ImageData { get; set; } = [];
}

