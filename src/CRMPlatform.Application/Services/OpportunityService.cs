using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Services;

public class OpportunityService : IOpportunityService
{
    private readonly IApplicationDbContext _context;

    public OpportunityService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync(ClaimsPrincipal user)
    {
        return await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Customer)
            .ApplyUserScope(user)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<Opportunity?> GetOpportunityByIdAsync(int id, ClaimsPrincipal user)
    {
        return await _context.Opportunities
            .Include(o => o.Customer)
            .ApplyUserScope(user)
            .FirstOrDefaultAsync(o => o.OpportunityId == id);
    }

    public async Task<Opportunity> CreateOpportunityAsync(Opportunity opportunity, ClaimsPrincipal user)
    {
        ValidateOpportunityBusinessRules(opportunity);

        opportunity.AssignedTo = user.FindFirstValue(ClaimTypes.NameIdentifier);
        opportunity.CreatedDate = DateTime.UtcNow;

        _context.Opportunities.Add(opportunity);
        await _context.SaveChangesAsync();

        return opportunity;
    }

    public async Task UpdateOpportunityAsync(Opportunity opportunity, ClaimsPrincipal user)
    {
        var existing = await _context.Opportunities
            .ApplyUserScope(user)
            .FirstOrDefaultAsync(o => o.OpportunityId == opportunity.OpportunityId);

        if (existing == null)
        {
            throw new BusinessRuleException(nameof(opportunity.OpportunityId), "Opportunity was not found or access is denied.");
        }

        ValidateOpportunityBusinessRules(opportunity);

        existing.OpportunityName = opportunity.OpportunityName;
        existing.Amount = opportunity.Amount;
        existing.Probability = opportunity.Probability;
        existing.Stage = opportunity.Stage;
        existing.ExpectedCloseDate = opportunity.ExpectedCloseDate;
        existing.CustomerId = opportunity.CustomerId;

        await _context.SaveChangesAsync();
    }

    private static void ValidateOpportunityBusinessRules(Opportunity opportunity)
    {
        bool isActive = opportunity.Stage != "Closed Lost" && opportunity.Stage != "Closed Won";

        // Business Rule 1: Opportunity Amount > 0 for active opportunities
        if (isActive && opportunity.Amount <= 0)
        {
            throw new BusinessRuleException(nameof(opportunity.Amount), "Opportunity amount must be greater than 0 for active opportunities.");
        }

        // Business Rule 2: Probability between 0 and 100
        if (opportunity.Probability < 0 || opportunity.Probability > 100)
        {
            throw new BusinessRuleException(nameof(opportunity.Probability), "Win probability must be between 0% and 100%.");
        }

        // Business Rule 3: Expected Close Date cannot be in the past for active opportunities
        if (isActive && opportunity.ExpectedCloseDate.HasValue && opportunity.ExpectedCloseDate.Value.Date < DateTime.UtcNow.Date)
        {
            throw new BusinessRuleException(nameof(opportunity.ExpectedCloseDate), "Expected close date cannot be in the past for active opportunities.");
        }
    }
}