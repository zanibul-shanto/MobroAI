using System.ComponentModel.DataAnnotations;

namespace MobroLens.Models;

public class User : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public Role Role { get; set; } = Role.Parent;

    public string? PasswordResetCode { get; set; }
    public DateTime? PasswordResetCodeExpiry { get; set; }
}
