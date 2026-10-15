using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Application.DTOs;

public record FollowUpResponseDto(
    int FollowUpId,
    string Subject,
    int? CustomerId,
    int? LeadId,
    int? OpportunityId,
    string FollowUpType,
    DateTime ScheduledDate,
    string Status,
    string? Notes,
    string? AssignedTo
);

public class CreateFollowUpDto
{
    [Required]
    [StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    public int? CustomerId { get; set; }
    public int? LeadId { get; set; }
    public int? OpportunityId { get; set; }

    [Required]
    public string FollowUpType { get; set; } = "Call";

    [Required]
    public DateTime ScheduledDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}