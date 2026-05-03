namespace MorboLensAI.Models;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum Role
{
    Admin = 0,
    HealthCareOfficer = 1,
    Parent = 2
}

public enum ScanStatus
{
    Pending = 0,
    AI_Confirmed = 1,
    Officer_Verified = 2,
    Cleared = 3
}