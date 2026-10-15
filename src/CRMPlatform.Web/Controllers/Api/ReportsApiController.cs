using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.DTOs;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;

namespace CRMPlatform.Web.Controllers.Api;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
public class ReportsApiController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public ReportsApiController(IApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /api/reports/pipeline
    [HttpGet("pipeline")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PipelineReportResponseDto))]
    public async Task<IActionResult> GetPipelineReport()
    {
        var opps = await _context.Opportunities
            .AsNoTracking()
            .ApplyUserScope(User)
            .ToListAsync();

        var stages = new[] { "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" };

        var stageSummaries = stages.Select(stage =>
        {
            var deals = opps.Where(o => string.Equals(o.Stage, stage, StringComparison.OrdinalIgnoreCase)).ToList();
            var totalVal = deals.Sum(d => d.Amount);
            var weightedVal = deals.Sum(d => (d.Amount * d.Probability) / 100m);
            return new PipelineStageSummaryDto(stage, deals.Count, totalVal, weightedVal);
        }).ToList();

        var activeOpps = opps.Where(o => o.Stage != "Closed Lost" && o.Stage != "Lost").ToList();
        var totalPipeline = activeOpps.Sum(o => o.Amount);
        var totalWeighted = activeOpps.Sum(o => (o.Amount * o.Probability) / 100m);

        var report = new PipelineReportResponseDto(
            totalPipeline,
            totalWeighted,
            activeOpps.Count,
            stageSummaries);

        return Ok(report);
    }
}