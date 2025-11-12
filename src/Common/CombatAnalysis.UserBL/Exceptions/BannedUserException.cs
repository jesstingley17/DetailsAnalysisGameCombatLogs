namespace CombatAnalysis.UserBL.Exceptions;

public class BannedUserException(int bannedUserId) : Exception("User could not ban himself.")
{
    public int BannedUserId { get; } = bannedUserId;

    public static void ThrowIfEquals(string userId1, string userId2)
    {
        if (string.Equals(userId1, userId2, StringComparison.OrdinalIgnoreCase))
        {
            throw new BannedUserException(0);
        }
    }
}
