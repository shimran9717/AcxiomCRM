namespace CRMPlatform.Application.Exceptions;

public class BusinessRuleException : Exception
{
    public string PropertyName { get; }

    public BusinessRuleException(string propertyName, string message) : base(message)
    {
        PropertyName = propertyName;
    }
}