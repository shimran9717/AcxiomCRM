namespace CRMPlatform.Domain.Entities;

public class FollowUp
{
    public int FollowUpId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string FollowUpType { get; set; } = "Call"; // Call, Email, Meeting
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "Planned"; // Planned, Completed, Cancelled
    public string? Notes { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? LeadId { get; set; }
    public Lead? Lead { get; set; }

    public int? OpportunityId { get; set; }
    public Opportunity? Opportunity { get; set; }
}