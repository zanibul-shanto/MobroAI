using System.ComponentModel.DataAnnotations;

namespace MorboLensAI.Models;

public class Child : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ParentId { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }
}

