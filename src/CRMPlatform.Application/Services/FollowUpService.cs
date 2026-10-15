using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Services;

public class FollowUpService : IFollowUpService
{
    private readonly IApplicationDbContext _context;

    public FollowUpService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FollowUp>> GetUpcomingFollowUpsAsync(ClaimsPrincipal user)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.FollowUps
            .AsNoTracking()
            .Include(f => f.Customer)
            .Include(f => f.Lead)
            .ApplyUserScope(user)
            .Where(f => f.Status == "Planned" && f.ScheduledDate >= today)
            .OrderBy(f => f.ScheduledDate)
            .Take(50)
            .ToListAsync();
    }

    public async Task<IEnumerable<FollowUp>> GetOverdueFollowUpsAsync(ClaimsPrincipal user)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.FollowUps
            .AsNoTracking()
            .Include(f => f.Customer)
            .Include(f => f.Lead)
            .ApplyUserScope(user)
            .Where(f => f.Status == "Planned" && f.ScheduledDate < today)
            .OrderBy(f => f.ScheduledDate)
            .Take(50)
            .ToListAsync();
    }

    public async Task<IEnumerable<Activity>> GetRecentActivitiesAsync(ClaimsPrincipal user)
    {
        return await _context.Activities
            .AsNoTracking()
            .ApplyUserScope(user)
            .OrderByDescending(a => a.ActivityDate)
            .Take(25)
            .ToListAsync();
    }

    public async Task<FollowUp> ScheduleFollowUpAsync(FollowUp followUp, ClaimsPrincipal user)
    {
        if (followUp.ScheduledDate.Date < DateTime.UtcNow.Date)
            throw new BusinessRuleException(nameof(followUp.ScheduledDate), "Scheduled date cannot be in the past.");

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        followUp.AssignedTo = userId;
        followUp.CreatedDate = DateTime.UtcNow;
        followUp.Status = "Planned";

        _context.FollowUps.Add(followUp);

        _context.Activities.Add(new Activity
        {
            ActivityType = "FollowUp Scheduled",
            Description = $"Scheduled {followUp.FollowUpType}: {followUp.Subject}",
            ActivityDate = DateTime.UtcNow,
            CustomerId = followUp.CustomerId,
            LeadId = followUp.LeadId,
            OpportunityId = followUp.OpportunityId,
            PerformedBy = userId
        });

        await _context.SaveChangesAsync();
        return followUp;
    }

    public async Task CompleteFollowUpAsync(int followUpId, string? outcomeNotes, ClaimsPrincipal user)
    {
        var followUp = await _context.FollowUps.ApplyUserScope(user).FirstOrDefaultAsync(f => f.FollowUpId == followUpId);
        if (followUp == null)
            throw new BusinessRuleException(nameof(followUpId), "Follow-up record was not found.");

        followUp.Status = "Completed";
        if (!string.IsNullOrWhiteSpace(outcomeNotes))
            followUp.Notes = $"{followUp.Notes} | Outcome: {outcomeNotes}".Trim();

        _context.Activities.Add(new Activity
        {
            ActivityType = "FollowUp Completed",
            Description = $"Completed {followUp.FollowUpType}: {followUp.Subject}",
            ActivityDate = DateTime.UtcNow,
            CustomerId = followUp.CustomerId,
            LeadId = followUp.LeadId,
            OpportunityId = followUp.OpportunityId,
            PerformedBy = user.FindFirstValue(ClaimTypes.NameIdentifier)
        });

        await _context.SaveChangesAsync();
    }
}