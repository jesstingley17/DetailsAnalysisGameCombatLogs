namespace CombatAnalysis.UserBL.Exceptions;

public class RequestToConnectException(int requestToConenctId) : Exception("User could not request himself.")
{
    public int RequestToConenctId { get; } = requestToConenctId;

    public static void ThrowIfEquals(string userId1, string userId2)
    {
        if (string.Equals(userId1, userId2, StringComparison.OrdinalIgnoreCase))
        {
            throw new RequestToConnectException(0);
        }
    }
}
