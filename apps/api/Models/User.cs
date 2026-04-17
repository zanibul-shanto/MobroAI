using System.ComponentModel.DataAnnotations;

namespace MorboLensAI.Models;

public class User
{
    public int Id { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }
    public string? UserType { get; set;}
    public long? MobileNo { get; set; }
    public long? NID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
