using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Web.Models.ViewModels;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class DashboardController : Controller
{
    private readonly IApplicationDbContext _context;

    public DashboardController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel();

        // 1. Scoped Queryables
        var customersQuery = _context.Customers.AsNoTracking().ApplyUserScope(User);
        var leadsQuery = _context.Leads.AsNoTracking().ApplyUserScope(User);
        var oppsQuery = _context.Opportunities.AsNoTracking().ApplyUserScope(User);

        // 2. KPI Summary Aggregations
        model.TotalCustomers = await customersQuery.CountAsync();
        model.TotalLeads = await leadsQuery.CountAsync();
        model.OpenLeads = await leadsQuery.CountAsync(l => l.Status == "New" || l.Status == "Contacted" || l.Status == "Qualified");

        var allOpportunities = await oppsQuery.ToListAsync();
        model.TotalOpportunities = allOpportunities.Count;
        model.WonOpportunities = allOpportunities.Count(o => o.Stage == "Closed Won" || o.Stage == "Won");
        model.LostOpportunities = allOpportunities.Count(o => o.Stage == "Closed Lost" || o.Stage == "Lost");
        model.OpenOpportunities = model.TotalOpportunities - (model.WonOpportunities + model.LostOpportunities);

        model.TotalPipelineValue = allOpportunities
            .Where(o => o.Stage != "Closed Lost" && o.Stage != "Lost")
            .Sum(o => o.Amount);

        model.WeightedPipelineValue = allOpportunities
            .Where(o => o.Stage != "Closed Lost" && o.Stage != "Lost")
            .Sum(o => (o.Amount * o.Probability) / 100m);

        // 3. Chart 1: Lead Status Breakdown
        var leadStatuses = new[] { "New", "Contacted", "Qualified", "Converted", "Lost" };
        var leadGroupings = await leadsQuery
            .GroupBy(l => l.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        foreach (var status in leadStatuses)
        {
            model.LeadStatusLabels.Add(status);
            var match = leadGroupings.FirstOrDefault(g => string.Equals(g.Status, status, StringComparison.OrdinalIgnoreCase));
            model.LeadStatusCounts.Add(match?.Count ?? 0);
        }

        // 4. Chart 2: Pipeline Funnel by Stage
        var stages = new[] { "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" };
        foreach (var stage in stages)
        {
            model.StageLabels.Add(stage);
            var dealsInStage = allOpportunities.Where(o => string.Equals(o.Stage, stage, StringComparison.OrdinalIgnoreCase)).ToList();
            model.StageValues.Add(dealsInStage.Sum(d => d.Amount));
            model.StageCounts.Add(dealsInStage.Count);
        }

        // 5. Chart 3: Monthly Sales Outcomes (Trailing 6 Months)
        var now = DateTime.UtcNow;
        for (int i = 5; i >= 0; i--)
        {
            var targetMonth = now.AddMonths(-i);
            var monthLabel = targetMonth.ToString("MMM yyyy");
            model.MonthlyLabels.Add(monthLabel);

            var monthWonRevenue = allOpportunities
                .Where(o => (o.Stage == "Closed Won" || o.Stage == "Won") &&
                            o.ExpectedCloseDate.HasValue &&
                            o.ExpectedCloseDate.Value.Year == targetMonth.Year &&
                            o.ExpectedCloseDate.Value.Month == targetMonth.Month)
                .Sum(o => o.Amount);

            model.MonthlyWonRevenue.Add(monthWonRevenue);
        }

        return View(model);
    }
}