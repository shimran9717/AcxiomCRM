using Microsoft.EntityFrameworkCore;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Lead> Leads { get; }
    DbSet<Opportunity> Opportunities { get; }
    DbSet<FollowUp> FollowUps { get; }
    DbSet<Activity> Activities { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}