using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CRMPlatform.Application.Common;
using CRMPlatform.Application.Exceptions;
using CRMPlatform.Application.Interfaces;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IApplicationDbContext _context;

    public CustomerService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync(ClaimsPrincipal user)
    {
        return await _context.Customers
            .AsNoTracking()
            .ApplyUserScope(user)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id, ClaimsPrincipal user)
    {
        return await _context.Customers
            .ApplyUserScope(user)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer, ClaimsPrincipal user)
    {
        var emailExists = await _context.Customers
            .AnyAsync(c => c.Email.ToLower() == customer.Email.ToLower());

        if (emailExists)
            throw new BusinessRuleException(nameof(customer.Email), "A customer with this email address already exists.");

        var phoneExists = await _context.Customers
            .AnyAsync(c => c.Phone == customer.Phone);

        if (phoneExists)
            throw new BusinessRuleException(nameof(customer.Phone), "A customer with this phone number already exists.");

        customer.OwnerId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        customer.CreatedDate = DateTime.UtcNow;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task UpdateCustomerAsync(Customer customer, ClaimsPrincipal user)
    {
        var existing = await _context.Customers
            .ApplyUserScope(user)
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        if (existing == null)
            throw new BusinessRuleException(nameof(customer.CustomerId), "Customer record was not found or access is denied.");

        var duplicateEmail = await _context.Customers
            .AnyAsync(c => c.Email.ToLower() == customer.Email.ToLower() && c.CustomerId != customer.CustomerId);

        if (duplicateEmail)
            throw new BusinessRuleException(nameof(customer.Email), "Another customer with this email address already exists.");

        var duplicatePhone = await _context.Customers
            .AnyAsync(c => c.Phone == customer.Phone && c.CustomerId != customer.CustomerId);

        if (duplicatePhone)
            throw new BusinessRuleException(nameof(customer.Phone), "Another customer with this phone number already exists.");

        existing.CompanyName = customer.CompanyName;
        existing.CustomerName = customer.CustomerName;
        existing.Email = customer.Email;
        existing.Phone = customer.Phone;
        existing.Address = customer.Address;
        existing.Status = customer.Status;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id, ClaimsPrincipal user)
    {
        var customer = await _context.Customers
            .ApplyUserScope(user)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}