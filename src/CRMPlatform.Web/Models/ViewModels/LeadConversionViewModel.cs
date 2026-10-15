using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Web.Models.ViewModels;

public class LeadConversionViewModel
{
    public int LeadId { get; set; }

    [Display(Name = "Lead Name")]
    public string LeadName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required.")]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact name is required.")]
    [Display(Name = "Contact Name")]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required.")]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Generate an Initial Opportunity?")]
    public bool CreateOpportunity { get; set; } = true;

    [Display(Name = "Opportunity Name")]
    public string? OpportunityName { get; set; }

    [Range(0.01, 999999999.99)]
    [Display(Name = "Estimated Deal Amount")]
    public decimal? OpportunityAmount { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Expected Close Date")]
    public DateTime? ExpectedCloseDate { get; set; } = DateTime.UtcNow.AddDays(30);
}