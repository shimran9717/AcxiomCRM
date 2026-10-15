namespace CRMPlatform.Domain.Entities;

public class Opportunity
{
    public int OpportunityId { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public decimal Amount { get; set; }
    public int Probability { get; set; } 
    public string Stage { get; set; } = "Qualification"; 
    public DateTime? ExpectedCloseDate { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
}