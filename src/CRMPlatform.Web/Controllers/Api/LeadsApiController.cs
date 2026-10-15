using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.DTOs;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Web.Controllers.Api;

[ApiController]
[Route("api/leads")]
[Produces("application/json")]
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class LeadsApiController : ControllerBase
{
    private readonly ILeadService _leadService;
    private readonly IApplicationDbContext _context;

    public LeadsApiController(ILeadService leadService, IApplicationDbContext context)
    {
        _leadService = leadService;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LeadResponseDto>))]
    public async Task<IActionResult> GetAll()
    {
        var leads = await _context.Leads
            .AsNoTracking()
            .ApplyUserScope(User)
            .OrderByDescending(l => l.CreatedDate)
            .Select(l => new LeadResponseDto(
                l.LeadId,
                l.LeadName,
                l.CompanyName ?? string.Empty,
                l.Email,
                l.Phone,
                l.Status,
                l.Source,
                l.EstimatedValue,
                l.AssignedTo,
                l.CreatedDate))
            .ToListAsync();

        return Ok(leads);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LeadResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLeadDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var lead = new Lead
        {
            LeadName = dto.LeadName,
            CompanyName = dto.CompanyName,
            Email = dto.Email,
            Phone = dto.Phone,
            Source = dto.Source,
            EstimatedValue = dto.EstimatedValue
        };

        var created = await _leadService.CreateLeadAsync(lead, User);

        var response = new LeadResponseDto(
            created.LeadId,
            created.LeadName,
            created.CompanyName ?? string.Empty,
            created.Email,
            created.Phone,
            created.Status,
            created.Source,
            created.EstimatedValue,
            created.AssignedTo,
            created.CreatedDate);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}