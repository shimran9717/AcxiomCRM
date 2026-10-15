using System.Security.Claims;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Interfaces;

public interface IFollowUpService
{
    Task<IEnumerable<FollowUp>> GetUpcomingFollowUpsAsync(ClaimsPrincipal user);
    Task<IEnumerable<FollowUp>> GetOverdueFollowUpsAsync(ClaimsPrincipal user);
    Task<IEnumerable<Activity>> GetRecentActivitiesAsync(ClaimsPrincipal user);
    Task<FollowUp> ScheduleFollowUpAsync(FollowUp followUp, ClaimsPrincipal user);
    Task CompleteFollowUpAsync(int followUpId, string? outcomeNotes, ClaimsPrincipal user);
}