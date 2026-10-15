using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Web.Models.ViewModels;

public class FollowUpViewModel
{
    public int FollowUpId { get; set; }

    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Customer")]
    public int? CustomerId { get; set; }

    [Display(Name = "Lead")]
    public int? LeadId { get; set; }

    [Display(Name = "Opportunity")]
    public int? OpportunityId { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public string Type { get; set; } = "Call";

    [Required(ErrorMessage = "Scheduled date is required.")]
    [DataType(DataType.DateTime)]
    public DateTime ScheduledDate { get; set; } = DateTime.UtcNow.AddDays(1);

    [StringLength(500)]
    public string? Notes { get; set; }
}