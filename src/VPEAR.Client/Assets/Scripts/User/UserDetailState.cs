using VPEAR.Core.Wrappers;

public class UserDetailState
{
    public UserDetailState(bool isLoading, GetUserResponse user)
    {
        IsLoading = isLoading;
        User = user;
    }

    public bool IsLoading { get; }

    public GetUserResponse User { get; }
}

