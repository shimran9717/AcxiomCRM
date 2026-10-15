using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.DTOs;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Web.Controllers.Api;

[ApiController]
[Route("api/opportunities")]
[Produces("application/json")]
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class OpportunitiesApiController : ControllerBase
{
    private readonly IOpportunityService _opportunityService;
    private readonly IApplicationDbContext _context;

    public OpportunitiesApiController(IOpportunityService opportunityService, IApplicationDbContext context)
    {
        _opportunityService = opportunityService;
        _context = context;
    }

    // GET: /api/opportunities
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OpportunityResponseDto>))]
    public async Task<IActionResult> GetAll()
    {
        var opps = await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Customer)
            .ApplyUserScope(User)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OpportunityResponseDto(
                o.OpportunityId,
                o.OpportunityName,
                o.CustomerId,
                o.Customer != null ? o.Customer.CompanyName : "N/A",
                o.Amount,
                o.Probability,
                o.Stage,
                o.ExpectedCloseDate,
                o.AssignedTo,
                o.CreatedDate))
            .ToListAsync();

        return Ok(opps);
    }

    // POST: /api/opportunities
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OpportunityResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOpportunityDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var opportunity = new Opportunity
        {
            OpportunityName = dto.OpportunityName,
            CustomerId = dto.CustomerId,
            Amount = dto.Amount,
            Probability = dto.Probability,
            Stage = dto.Stage,
            ExpectedCloseDate = dto.ExpectedCloseDate.HasValue 
                ? DateTime.SpecifyKind(dto.ExpectedCloseDate.Value, DateTimeKind.Utc) 
                : null
        };

        try
        {
            var created = await _opportunityService.CreateOpportunityAsync(opportunity, User);
            var response = new OpportunityResponseDto(
                created.OpportunityId,
                created.OpportunityName,
                created.CustomerId,
                created.Customer?.CompanyName ?? "N/A",
                created.Amount,
                created.Probability,
                created.Stage,
                created.ExpectedCloseDate,
                created.AssignedTo,
                created.CreatedDate);

            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { property = ex.PropertyName, message = ex.Message });
        }
    }
}