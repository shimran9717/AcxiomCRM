using System.Security.Claims;
using CRMPlatform.Application.Common;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Interfaces;

public interface ILeadService
{
    Task<PaginatedList<Lead>> GetLeadsAsync(string? search, string? status, int pageIndex, int pageSize, ClaimsPrincipal user);
    Task<Lead?> GetLeadByIdAsync(int id, ClaimsPrincipal user);
    Task<Lead> CreateLeadAsync(Lead lead, ClaimsPrincipal user);
    Task UpdateLeadAsync(Lead lead, ClaimsPrincipal user);
    Task<int> ConvertLeadAsync(
        int leadId,
        string companyName,
        string contactName,
        string email,
        string phone,
        bool createOpportunity,
        string? oppTitle,
        decimal? oppAmount,
        DateTime? oppCloseDate,
        ClaimsPrincipal user);
}