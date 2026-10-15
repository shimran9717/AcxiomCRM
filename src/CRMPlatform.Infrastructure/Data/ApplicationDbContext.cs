using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // AuditLog Configuration & Indexes
        builder.Entity<AuditLog>(b =>
        {
            b.HasKey(a => a.AuditLogId);
            b.Property(a => a.Action).HasMaxLength(50).IsRequired();
            b.Property(a => a.EntityName).HasMaxLength(100).IsRequired();
            b.Property(a => a.RecordId).HasMaxLength(100).IsRequired();
            b.Property(a => a.UserId).HasMaxLength(128);
            b.Property(a => a.IpAddress).HasMaxLength(45); // IPv4 / IPv6
            b.HasIndex(a => a.Timestamp);
            b.HasIndex(a => a.EntityName);
            b.HasIndex(a => a.UserId);
        });

        // Decimal precision configurations
        builder.Entity<Opportunity>().Property(o => o.Amount).HasPrecision(18, 2);
        builder.Entity<Lead>().Property(l => l.EstimatedValue).HasPrecision(18, 2);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Enforce Append-Only: Throw exception if someone attempts to modify or delete audit logs
        var tamperedAuditEntries = ChangeTracker.Entries<AuditLog>()
            .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        if (tamperedAuditEntries.Count != 0)
        {
            throw new InvalidOperationException("Audit logs are append-only. Modification and deletion of audit records are strictly prohibited.");
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}