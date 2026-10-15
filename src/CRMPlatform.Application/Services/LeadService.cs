using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Services;

public class LeadService : ILeadService
{
    private readonly IApplicationDbContext _context;

    public LeadService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Lead>> GetLeadsAsync(string? search, string? status, int pageIndex, int pageSize, ClaimsPrincipal user)
    {
        var query = _context.Leads.AsNoTracking().ApplyUserScope(user).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(l =>
                l.LeadName.ToLower().Contains(term) ||
                (l.CompanyName != null && l.CompanyName.ToLower().Contains(term)) ||
                l.Email.ToLower().Contains(term) ||
                l.Phone.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(l => l.Status == status);
        }

        query = query.OrderByDescending(l => l.CreatedDate);
        return await PaginatedList<Lead>.CreateAsync(query, pageIndex, pageSize);
    }

    public async Task<Lead?> GetLeadByIdAsync(int id, ClaimsPrincipal user)
    {
        return await _context.Leads.ApplyUserScope(user).FirstOrDefaultAsync(l => l.LeadId == id);
    }

    public async Task<Lead> CreateLeadAsync(Lead lead, ClaimsPrincipal user)
    {
        lead.AssignedTo = user.FindFirstValue(ClaimTypes.NameIdentifier);
        lead.CreatedDate = DateTime.UtcNow;
        lead.Status = "New";

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();
        return lead;
    }

    public async Task UpdateLeadAsync(Lead lead, ClaimsPrincipal user)
    {
        var existing = await _context.Leads.ApplyUserScope(user).FirstOrDefaultAsync(l => l.LeadId == lead.LeadId);
        if (existing == null)
            throw new BusinessRuleException(nameof(lead.LeadId), "Lead not found or access denied.");

        if (existing.Status == "Converted" && lead.Status != "Converted")
            throw new BusinessRuleException(nameof(lead.Status), "Cannot modify status of an already converted lead.");

        existing.LeadName = lead.LeadName;
        existing.CompanyName = lead.CompanyName;
        existing.Email = lead.Email;
        existing.Phone = lead.Phone;
        existing.Status = lead.Status;
        existing.Source = lead.Source;
        existing.EstimatedValue = lead.EstimatedValue;

        await _context.SaveChangesAsync();
    }

    public async Task<int> ConvertLeadAsync(
        int leadId,
        string companyName,
        string contactName,
        string email,
        string phone,
        bool createOpportunity,
        string? oppTitle,
        decimal? oppAmount,
        DateTime? oppCloseDate,
        ClaimsPrincipal user)
    {
        var lead = await _context.Leads.ApplyUserScope(user).FirstOrDefaultAsync(l => l.LeadId == leadId);
        if (lead == null)
            throw new BusinessRuleException(nameof(leadId), "Lead not found or access denied.");

        if (lead.Status == "Converted")
            throw new BusinessRuleException(nameof(lead.Status), "This lead has already been converted.");

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        var customer = new Customer
        {
            CompanyName = companyName,
            CustomerName = contactName,
            Email = email,
            Phone = phone,
            OwnerId = userId,
            CreatedDate = DateTime.UtcNow,
            Status = "Active"
        };
        _context.Customers.Add(customer);

        lead.Status = "Converted";

        if (createOpportunity)
        {
            var opportunity = new Opportunity
            {
                Customer = customer,
                OpportunityName = string.IsNullOrWhiteSpace(oppTitle) ? $"{companyName} Deal" : oppTitle,
                Amount = oppAmount ?? lead.EstimatedValue ?? 1000m,
                Probability = 25,
                Stage = "Qualification",
                ExpectedCloseDate = oppCloseDate.HasValue ? DateTime.SpecifyKind(oppCloseDate.Value, DateTimeKind.Utc) : DateTime.UtcNow.AddDays(30),
                AssignedTo = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.Opportunities.Add(opportunity);
        }

        await _context.SaveChangesAsync();
        return customer.CustomerId;
    }
}