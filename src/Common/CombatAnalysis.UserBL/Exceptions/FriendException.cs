namespace CombatAnalysis.UserBL.Exceptions;

public class FriendException(int friendId) : Exception("User could not be friend himself.")
{
    public int FriendId { get; } = friendId;

    public static void ThrowIfEquals(string userId1, string userId2)
    {
        if (string.Equals(userId1, userId2, StringComparison.OrdinalIgnoreCase))
        {
            throw new FriendException(0);
        }
    }
}
