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
[Route("api/followups")]
[Produces("application/json")]
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class FollowUpsApiController : ControllerBase
{
    private readonly IFollowUpService _followUpService;
    private readonly IApplicationDbContext _context;

    public FollowUpsApiController(IFollowUpService followUpService, IApplicationDbContext context)
    {
        _followUpService = followUpService;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowUpResponseDto>))]
    public async Task<IActionResult> GetAll()
    {
        var followups = await _context.FollowUps
            .AsNoTracking()
            .ApplyUserScope(User)
            .OrderByDescending(f => f.ScheduledDate)
            .Select(f => new FollowUpResponseDto(
                f.FollowUpId,
                f.Subject,
                f.CustomerId,
                f.LeadId,
                f.OpportunityId,
                f.FollowUpType,
                f.ScheduledDate,
                f.Status,
                f.Notes,
                f.AssignedTo))
            .ToListAsync();

        return Ok(followups);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FollowUpResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFollowUpDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var followUp = new FollowUp
        {
            Subject = dto.Subject,
            CustomerId = dto.CustomerId,
            LeadId = dto.LeadId,
            OpportunityId = dto.OpportunityId,
            FollowUpType = dto.FollowUpType,
            ScheduledDate = DateTime.SpecifyKind(dto.ScheduledDate, DateTimeKind.Utc),
            Notes = dto.Notes,
            Status = "Scheduled"
        };

        try
        {
            var created = await _followUpService.ScheduleFollowUpAsync(followUp, User);
            var response = new FollowUpResponseDto(
                created.FollowUpId,
                created.Subject,
                created.CustomerId,
                created.LeadId,
                created.OpportunityId,
                created.FollowUpType,
                created.ScheduledDate,
                created.Status,
                created.Notes,
                created.AssignedTo);

            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { property = ex.PropertyName, message = ex.Message });
        }
    }
}