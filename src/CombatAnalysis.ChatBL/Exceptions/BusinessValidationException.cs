namespace CombatAnalysis.ChatBL.Exceptions;

public class BusinessValidationException : Exception
{
    public string? ErrorCode { get; }

    public BusinessValidationException() { }

    public BusinessValidationException(string message)
        : base(message) { }

    public BusinessValidationException(string message, string? errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public BusinessValidationException(string message, Exception innerException)
        : base(message, innerException) { }
}
