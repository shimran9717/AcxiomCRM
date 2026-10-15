using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;
using CRMPlatform.Web.Models.ViewModels;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class ActivitiesController : Controller
{
    private readonly IFollowUpService _followUpService;
    private readonly ICustomerService _customerService;

    public ActivitiesController(IFollowUpService followUpService, ICustomerService customerService)
    {
        _followUpService = followUpService;
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new ActivityDashboardViewModel
        {
            OverdueFollowUps = await _followUpService.GetOverdueFollowUpsAsync(User),
            UpcomingFollowUps = await _followUpService.GetUpcomingFollowUpsAsync(User),
            RecentActivities = await _followUpService.GetRecentActivitiesAsync(User)
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Schedule(int? customerId, int? leadId, int? opportunityId)
    {
        await PopulateCustomerDropdownAsync(customerId);
        return View(new FollowUpViewModel
        {
            CustomerId = customerId,
            LeadId = leadId,
            OpportunityId = opportunityId,
            ScheduledDate = DateTime.UtcNow.Date.AddDays(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Schedule(FollowUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }

        var followUp = new FollowUp
        {
            Subject = model.Subject,
            CustomerId = model.CustomerId,
            LeadId = model.LeadId,
            OpportunityId = model.OpportunityId,
            FollowUpType = model.Type,
            ScheduledDate = DateTime.SpecifyKind(model.ScheduledDate, DateTimeKind.Utc),
            Notes = model.Notes
        };

        try
        {
            await _followUpService.ScheduleFollowUpAsync(followUp, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            await PopulateCustomerDropdownAsync(model.CustomerId);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id, string? outcomeNotes)
    {
        try
        {
            await _followUpService.CompleteFollowUpAsync(id, outcomeNotes, User);
        }
        catch (BusinessRuleException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCustomerDropdownAsync(int? selectedId = null)
    {
        var customers = await _customerService.GetAllCustomersAsync(User);
        ViewBag.CustomerId = new SelectList(customers, "CustomerId", "CompanyName", selectedId);
    }
}