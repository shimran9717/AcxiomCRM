using System.Security.Claims;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync(ClaimsPrincipal user);
    Task<Customer?> GetCustomerByIdAsync(int id, ClaimsPrincipal user);
    Task<Customer> CreateCustomerAsync(Customer customer, ClaimsPrincipal user);
    Task UpdateCustomerAsync(Customer customer, ClaimsPrincipal user);
    Task DeleteCustomerAsync(int id, ClaimsPrincipal user);
}