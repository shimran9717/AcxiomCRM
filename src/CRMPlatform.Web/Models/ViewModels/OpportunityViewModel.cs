using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Web.Models.ViewModels;

public class OpportunityViewModel
{
    public int OpportunityId { get; set; }

    [Required(ErrorMessage = "Opportunity name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 120 characters.")]
    [Display(Name = "Opportunity Name")]
    public string OpportunityName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a customer.")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 999999999.99, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [Range(0, 100, ErrorMessage = "Probability must be between 0 and 100.")]
    public int Probability { get; set; } = 20;

    [Required(ErrorMessage = "Stage is required.")]
    public string Stage { get; set; } = "Qualification";

    [Required(ErrorMessage = "Expected close date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Expected Close Date")]
    public DateTime ExpectedCloseDate { get; set; } = DateTime.UtcNow.AddDays(30);
}