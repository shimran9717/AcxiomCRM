using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Application.DTOs;

public record CustomerResponseDto(
    int CustomerId,
    string CompanyName,
    string CustomerName,
    string Email,
    string Phone,
    string? Address,
    string Status,
    string? OwnerId,
    DateTime CreatedDate
);

public class CreateCustomerDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    public string Status { get; set; } = "Active";
}

public class UpdateCustomerDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    public string Status { get; set; } = "Active";
}