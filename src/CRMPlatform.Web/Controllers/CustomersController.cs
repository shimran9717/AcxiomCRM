using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;
using CRMPlatform.Web.Models.ViewModels;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IApplicationDbContext _context;

    public CustomersController(ICustomerService customerService, IApplicationDbContext context)
    {
        _customerService = customerService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        ViewData["CurrentFilter"] = search;
        const int pageSize = 10;

        var query = _context.Customers.AsNoTracking().ApplyUserScope(User).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c =>
                c.CustomerName.ToLower().Contains(term) ||
                c.CompanyName.ToLower().Contains(term) ||
                c.Email.ToLower().Contains(term) ||
                c.Phone.Contains(term));
        }

        query = query.OrderByDescending(c => c.CreatedDate);
        var paginated = await PaginatedList<Customer>.CreateAsync(query, page, pageSize);
        return View(paginated);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .Include(c => c.Opportunities)
            .Include(c => c.FollowUps)
            .ApplyUserScope(User)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }

    [HttpGet]
    public IActionResult Create() => View(new CustomerViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var customer = new Customer
        {
            CompanyName = model.CompanyName,
            CustomerName = model.CustomerName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            Status = model.Status
        };

        try
        {
            await _customerService.CreateCustomerAsync(customer, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, User);
        if (customer == null)
            return NotFound();

        return View(new CustomerViewModel
        {
            CustomerId = customer.CustomerId,
            CompanyName = customer.CompanyName,
            CustomerName = customer.CustomerName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            Status = customer.Status
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CustomerViewModel model)
    {
        if (id != model.CustomerId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var customer = new Customer
        {
            CustomerId = model.CustomerId,
            CompanyName = model.CompanyName,
            CustomerName = model.CustomerName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            Status = model.Status
        };

        try
        {
            await _customerService.UpdateCustomerAsync(customer, User);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View(model);
        }
    }
}