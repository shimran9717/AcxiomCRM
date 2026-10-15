using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Web.Models.ViewModels;

public class CustomerViewModel
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    public string Status { get; set; } = "Active";
}