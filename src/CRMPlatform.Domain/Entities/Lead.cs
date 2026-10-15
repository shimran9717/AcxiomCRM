namespace CRMPlatform.Domain.Entities;

public class Lead
{
    public int LeadId { get; set; }
    public string LeadName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = "New"; // New -> Contacted -> Qualified -> Converted / Lost
    public string? Source { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
}