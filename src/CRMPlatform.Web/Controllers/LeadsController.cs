using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;
using CRMPlatform.Web.Models.ViewModels;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class LeadsController : Controller
{
    private readonly ILeadService _leadService;

    public LeadsController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? status, int page = 1)
    {
        ViewData["CurrentSearch"] = search;
        ViewData["CurrentStatus"] = status;

        var leads = await _leadService.GetLeadsAsync(search, status, page, 10, User);
        return View(leads);
    }

    [HttpGet]
    public IActionResult Create() => View(new Lead());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lead lead)
    {
        if (!ModelState.IsValid)
            return View(lead);

        await _leadService.CreateLeadAsync(lead, User);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var lead = await _leadService.GetLeadByIdAsync(id, User);
        if (lead == null)
            return NotFound();

        return View(lead);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lead lead)
    {
        if (id != lead.LeadId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(lead);

        try
        {
            await _leadService.UpdateLeadAsync(lead, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View(lead);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Convert(int id)
    {
        var lead = await _leadService.GetLeadByIdAsync(id, User);
        if (lead == null)
            return NotFound();

        if (lead.Status != "Qualified")
        {
            TempData["ErrorMessage"] = "Only Qualified leads can be converted into Customers.";
            return RedirectToAction(nameof(Index));
        }

        var model = new LeadConversionViewModel
        {
            LeadId = lead.LeadId,
            LeadName = lead.LeadName,
            CompanyName = lead.CompanyName ?? $"{lead.LeadName} Enterprise",
            ContactName = lead.LeadName,
            Email = lead.Email,
            Phone = lead.Phone,
            OpportunityName = $"{lead.CompanyName ?? lead.LeadName} Deal",
            OpportunityAmount = lead.EstimatedValue ?? 5000m
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Convert(LeadConversionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var customerId = await _leadService.ConvertLeadAsync(
                model.LeadId,
                model.CompanyName,
                model.ContactName,
                model.Email,
                model.Phone,
                model.CreateOpportunity,
                model.OpportunityName,
                model.OpportunityAmount,
                model.ExpectedCloseDate,
                User);

            return RedirectToAction("Details", "Customers", new { id = customerId });
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View(model);
        }
    }
}