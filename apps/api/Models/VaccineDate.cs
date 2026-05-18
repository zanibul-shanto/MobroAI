using System.ComponentModel.DataAnnotations;

namespace MobroLens.Models;

public class VaccineDate : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ChildId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public string? Note { get; set; }
}
