using System.Security.Claims;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Interfaces;

public interface IOpportunityService
{
    Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync(ClaimsPrincipal user);
    Task<Opportunity?> GetOpportunityByIdAsync(int id, ClaimsPrincipal user);
    Task<Opportunity> CreateOpportunityAsync(Opportunity opportunity, ClaimsPrincipal user);
    Task UpdateOpportunityAsync(Opportunity opportunity, ClaimsPrincipal user);
}