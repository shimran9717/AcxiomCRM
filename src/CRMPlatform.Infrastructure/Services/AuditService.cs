using System.Text.Json;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;
using CRMPlatform.Infrastructure.Data;

namespace CRMPlatform.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogSecurityEventAsync(string action, string? username, string? details, string? ipAddress)
    {
        var log = new AuditLog
        {
            Action = action, // "Login Success" or "Login Failed"
            EntityName = "IdentityUser",
            RecordId = username ?? "Unknown",
            UserId = username ?? "Anonymous",
            Timestamp = DateTime.UtcNow,
            IpAddress = ipAddress ?? "127.0.0.1",
            NewValues = !string.IsNullOrWhiteSpace(details)
                ? JsonSerializer.Serialize(new { Info = details })
                : null
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}