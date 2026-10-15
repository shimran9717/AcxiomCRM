using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;
using CRMPlatform.Web.Models.ViewModels;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class OpportunitiesController : Controller
{
    private readonly IOpportunityService _opportunityService;
    private readonly ICustomerService _customerService;
    private readonly IApplicationDbContext _context;

    public OpportunitiesController(IOpportunityService opportunityService, ICustomerService customerService, IApplicationDbContext context)
    {
        _opportunityService = opportunityService;
        _customerService = customerService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? stage, int page = 1)
    {
        ViewData["CurrentStage"] = stage;
        var query = _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Customer)
            .ApplyUserScope(User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(stage))
            query = query.Where(o => o.Stage == stage);

        var list = await query.ToListAsync();

        // Weighted Pipeline Formula: Sum(Amount * Probability / 100)
        ViewBag.TotalPipeline = list.Sum(o => o.Amount);
        ViewBag.WeightedPipeline = list.Sum(o => (o.Amount * o.Probability) / 100m);

        var paginated = await PaginatedList<Opportunity>.CreateAsync(query.OrderByDescending(o => o.CreatedDate), page, 10);
        return View(paginated);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateCustomerDropdownAsync();
        return View(new OpportunityViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OpportunityViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }

        var opportunity = new Opportunity
        {
            OpportunityName = model.OpportunityName,
            CustomerId = model.CustomerId,
            Amount = model.Amount,
            Probability = model.Probability,
            Stage = model.Stage,
            ExpectedCloseDate = DateTime.SpecifyKind(model.ExpectedCloseDate, DateTimeKind.Utc)
        };

        try
        {
            await _opportunityService.CreateOpportunityAsync(opportunity, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var opp = await _opportunityService.GetOpportunityByIdAsync(id, User);
        if (opp == null) return NotFound();

        await PopulateCustomerDropdownAsync(opp.CustomerId);
        return View(new OpportunityViewModel
        {
            OpportunityId = opp.OpportunityId,
            OpportunityName = opp.OpportunityName,
            CustomerId = opp.CustomerId,
            Amount = opp.Amount,
            Probability = opp.Probability,
            Stage = opp.Stage,
            ExpectedCloseDate = opp.ExpectedCloseDate ?? DateTime.UtcNow.AddDays(14)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, OpportunityViewModel model)
    {
        if (id != model.OpportunityId) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }

        var opp = new Opportunity
        {
            OpportunityId = model.OpportunityId,
            OpportunityName = model.OpportunityName,
            CustomerId = model.CustomerId,
            Amount = model.Amount,
            Probability = model.Probability,
            Stage = model.Stage,
            ExpectedCloseDate = DateTime.SpecifyKind(model.ExpectedCloseDate, DateTimeKind.Utc)
        };

        try
        {
            await _opportunityService.UpdateOpportunityAsync(opp, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }
    }

    private async Task PopulateCustomerDropdownAsync(int? selectedId = null)
    {
        var customers = await _customerService.GetAllCustomersAsync(User);
        ViewBag.CustomerId = new SelectList(customers, "CustomerId", "CompanyName", selectedId);
    }
}