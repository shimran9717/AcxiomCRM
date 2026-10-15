using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Infrastructure.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private List<AuditEntry>? _pendingAuditEntries;

    public AuditLogInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        OnBeforeSaveChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        OnAfterSaveChanges(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        OnAfterSaveChanges(eventData.Context);
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void OnBeforeSaveChanges(DbContext? context)
    {
        if (context == null) return;

        context.ChangeTracker.DetectChanges();
        _pendingAuditEntries = new List<AuditEntry>();

        var userId = _currentUserService.UserId ?? "Anonymous";
        var ipAddress = _currentUserService.IpAddress ?? "127.0.0.1";

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                EntityName = entry.Entity.GetType().Name,
                UserId = userId,
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    auditEntry.Action = "Create";
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.Metadata.IsPrimaryKey())
                        {
                            auditEntry.KeyProperties.Add(prop);
                            continue;
                        }
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    break;

                case EntityState.Deleted:
                    auditEntry.Action = "Delete";
                    auditEntry.RecordId = GetPrimaryKeyString(entry);
                    foreach (var prop in entry.Properties)
                    {
                        auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                    }
                    break;

                case EntityState.Modified:
                    auditEntry.Action = "Update";
                    auditEntry.RecordId = GetPrimaryKeyString(entry);
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified)
                        {
                            auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                            auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        }
                    }
                    break;
            }

            _pendingAuditEntries.Add(auditEntry);
        }

        foreach (var entry in _pendingAuditEntries.Where(e => e.KeyProperties.Count == 0))
        {
            context.Set<AuditLog>().Add(entry.ToAuditLog());
        }
    }

    private void OnAfterSaveChanges(DbContext? context)
    {
        if (context == null || _pendingAuditEntries == null || _pendingAuditEntries.Count == 0)
            return;

        var entriesWithGeneratedKeys = _pendingAuditEntries.Where(e => e.KeyProperties.Count > 0).ToList();
        if (entriesWithGeneratedKeys.Count == 0) return;

        foreach (var entry in entriesWithGeneratedKeys)
        {
            entry.RecordId = string.Join(",", entry.KeyProperties.Select(p => p.CurrentValue?.ToString() ?? "0"));
            context.Set<AuditLog>().Add(entry.ToAuditLog());
        }

        _pendingAuditEntries = null;
        context.SaveChanges();
    }

    private static string GetPrimaryKeyString(EntityEntry entry)
    {
        var keyProps = entry.Properties.Where(p => p.Metadata.IsPrimaryKey()).Select(p => p.CurrentValue?.ToString());
        return string.Join(",", keyProps);
    }

    private sealed class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string? UserId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();
        public List<PropertyEntry> KeyProperties { get; } = new();

        public AuditLog ToAuditLog()
        {
            return new AuditLog
            {
                UserId = UserId,
                EntityName = EntityName,
                Action = Action,
                RecordId = RecordId,
                Timestamp = Timestamp,
                IpAddress = IpAddress,
                OldValues = OldValues.Count != 0 ? JsonSerializer.Serialize(OldValues) : null,
                NewValues = NewValues.Count != 0 ? JsonSerializer.Serialize(NewValues) : null
            };
        }
    }
}