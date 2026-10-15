namespace CRMPlatform.Domain.Entities;

public class Activity
{
    public int ActivityId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
    public string? PerformedBy { get; set; }

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? LeadId { get; set; }
    public Lead? Lead { get; set; }

    public int? OpportunityId { get; set; }
    public Opportunity? Opportunity { get; set; }
}