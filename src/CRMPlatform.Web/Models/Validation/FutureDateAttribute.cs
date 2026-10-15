using System.ComponentModel.DataAnnotations;

namespace CRMPlatform.Web.Models.Validation;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date.Date >= DateTime.UtcNow.Date;
        }
        return true;
    }
}