namespace CRMPlatform.Application.Interfaces;

public interface IAuditService
{
    Task LogSecurityEventAsync(string action, string? username, string? details, string? ipAddress);
}