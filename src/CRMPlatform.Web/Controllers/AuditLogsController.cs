using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Web.Controllers;

[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
public class AuditLogsController : Controller
{
    private readonly IApplicationDbContext _context;

    public AuditLogsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? entity, string? actionType, string? search, int page = 1)
    {
        ViewData["CurrentEntity"] = entity;
        ViewData["CurrentAction"] = actionType;
        ViewData["CurrentSearch"] = search;

        var query = _context.AuditLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(entity))
            query = query.Where(a => a.EntityName == entity);

        if (!string.IsNullOrWhiteSpace(actionType))
            query = query.Where(a => a.Action == actionType);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(a =>
                (a.UserId != null && a.UserId.ToLower().Contains(term)) ||
                a.RecordId.ToLower().Contains(term) ||
                (a.IpAddress != null && a.IpAddress.Contains(term)));
        }

        query = query.OrderByDescending(a => a.Timestamp);

        var paginated = await PaginatedList<AuditLog>.CreateAsync(query, page, 15);
        return View(paginated);
    }
}