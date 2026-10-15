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
[Route("api/customers")]
[Produces("application/json")]
[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.SalesExecutive}")]
public class CustomersApiController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IApplicationDbContext _context;

    public CustomersApiController(ICustomerService customerService, IApplicationDbContext context)
    {
        _customerService = customerService;
        _context = context;
    }

    // GET: /api/customers
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerResponseDto>))]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .ApplyUserScope(User)
            .OrderByDescending(c => c.CreatedDate)
            .Select(c => new CustomerResponseDto(
                c.CustomerId,
                c.CompanyName,
                c.CustomerName,
                c.Email,
                c.Phone,
                c.Address,
                c.Status,
                c.OwnerId,
                c.CreatedDate))
            .ToListAsync();

        return Ok(customers);
    }

    // GET: /api/customers/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .ApplyUserScope(User)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
            return NotFound(new { message = $"Customer with ID {id} not found or access denied." });

        var response = new CustomerResponseDto(
            customer.CustomerId,
            customer.CompanyName,
            customer.CustomerName,
            customer.Email,
            customer.Phone,
            customer.Address,
            customer.Status,
            customer.OwnerId,
            customer.CreatedDate);

        return Ok(response);
    }

    // POST: /api/customers
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = new Customer
        {
            CompanyName = dto.CompanyName,
            CustomerName = dto.CustomerName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            Status = dto.Status
        };

        try
        {
            var created = await _customerService.CreateCustomerAsync(customer, User);
            var response = new CustomerResponseDto(
                created.CustomerId,
                created.CompanyName,
                created.CustomerName,
                created.Email,
                created.Phone,
                created.Address,
                created.Status,
                created.OwnerId,
                created.CreatedDate);

            return CreatedAtAction(nameof(GetById), new { id = created.CustomerId }, response);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { property = ex.PropertyName, message = ex.Message });
        }
    }

    // PUT: /api/customers/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = new Customer
        {
            CustomerId = id,
            CompanyName = dto.CompanyName,
            CustomerName = dto.CustomerName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            Status = dto.Status
        };

        try
        {
            await _customerService.UpdateCustomerAsync(customer, User);
            return Ok(new { message = "Customer updated successfully." });
        }
        catch (BusinessRuleException ex)
        {
            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(new { message = ex.Message });

            return BadRequest(new { property = ex.PropertyName, message = ex.Message });
        }
    }

    // DELETE: /api/customers/{id}
    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers
            .ApplyUserScope(User)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
            return NotFound(new { message = $"Customer with ID {id} not found." });

        await _customerService.DeleteCustomerAsync(id, User);
        return Ok(new { message = $"Customer {id} deleted successfully." });
    }
}