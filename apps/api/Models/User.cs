using System.ComponentModel.DataAnnotations;

namespace MorboLensAI.Models;

public class User : BaseEntity
{
    public int Id { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }
    public Role Role { get; set; } = Role.User;
    public long? MobileNo { get; set; }
    public long? NID { get; set; }
}
