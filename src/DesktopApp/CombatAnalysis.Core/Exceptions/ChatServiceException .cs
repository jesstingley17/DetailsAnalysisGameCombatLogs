namespace CombatAnalysis.Core.Exceptions;

public class ChatServiceException : Exception
{
    public ChatServiceException(string message)
        : base(message)
    {
    }

    public ChatServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
