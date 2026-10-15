using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Application.DTOs;

public record LeadResponseDto(
    int LeadId,
    string LeadName,
    string CompanyName,
    string Email,
    string Phone,
    string Status,
    string? Source,
    decimal? EstimatedValue,
    string? AssignedTo,
    DateTime CreatedDate
);

public class CreateLeadDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LeadName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    public string? Source { get; set; } = "Website";

    [Range(0, 999999999.99)]
    public decimal? EstimatedValue { get; set; }
}