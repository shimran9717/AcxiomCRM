using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Application.DTOs;

public record OpportunityResponseDto(
    int OpportunityId,
    string OpportunityName,
    int CustomerId,
    string CustomerName,
    decimal Amount,
    int Probability,
    string Stage,
    DateTime? ExpectedCloseDate,
    string? AssignedTo,
    DateTime CreatedDate
);

public class CreateOpportunityDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string OpportunityName { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [Range(0.01, 999999999.99)]
    public decimal Amount { get; set; }

    [Range(0, 100)]
    public int Probability { get; set; } = 20;

    [Required]
    public string Stage { get; set; } = "Qualification";

    public DateTime? ExpectedCloseDate { get; set; }
}