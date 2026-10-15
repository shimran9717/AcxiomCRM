namespace CRMPlatform.Domain.Entities;

public class AuditLog
{
    public long AuditLogId { get; set; }
    public string? UserId { get; set; }
    public string Action { get; set; } = string.Empty; // Create, Update, Delete, Login Success, Login Failed
    public string EntityName { get; set; } = string.Empty;
    public string RecordId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
}