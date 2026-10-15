using System.Security.Claims;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Common;

public static class QueryExtensions
{
    public static IQueryable<Customer> ApplyUserScope(this IQueryable<Customer> query, ClaimsPrincipal user)
    {
        if (user.IsInRole(AppRoles.Admin) || user.IsInRole(AppRoles.Manager))
            return query;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return query.Where(c => c.OwnerId == userId);
    }

    public static IQueryable<Lead> ApplyUserScope(this IQueryable<Lead> query, ClaimsPrincipal user)
    {
        if (user.IsInRole(AppRoles.Admin) || user.IsInRole(AppRoles.Manager))
            return query;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return query.Where(l => l.AssignedTo == userId);
    }

    public static IQueryable<Opportunity> ApplyUserScope(this IQueryable<Opportunity> query, ClaimsPrincipal user)
    {
        if (user.IsInRole(AppRoles.Admin) || user.IsInRole(AppRoles.Manager))
            return query;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return query.Where(o => o.AssignedTo == userId);
    }

    public static IQueryable<FollowUp> ApplyUserScope(this IQueryable<FollowUp> query, ClaimsPrincipal user)
    {
        if (user.IsInRole(AppRoles.Admin) || user.IsInRole(AppRoles.Manager))
            return query;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return query.Where(f => f.AssignedTo == userId);
    }

    public static IQueryable<Activity> ApplyUserScope(this IQueryable<Activity> query, ClaimsPrincipal user)
    {
        if (user.IsInRole(AppRoles.Admin) || user.IsInRole(AppRoles.Manager))
            return query;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return query.Where(a => a.PerformedBy == userId);
    }
}