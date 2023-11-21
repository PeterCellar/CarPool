using CarPool.BL;


namespace CarPool.App.Messages;
public class UserSelectedMessage
{
    public UserDetailModel User { get; }

    public UserSelectedMessage(UserDetailModel user)
    {
        User = user;
    }
}
