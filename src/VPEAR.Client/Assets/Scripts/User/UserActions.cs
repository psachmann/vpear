using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class FetchingUsersAction
{
    public FetchingUsersAction(string role = default)
    {
        Role = role;
    }

    public string Role { get; }
}

public class FetchedUsersAction
{
    public FetchedUsersAction(IEnumerable<GetUserResponse> users, string role)
    {
        Users = users;
        Role = role;
    }

    public IEnumerable<GetUserResponse> Users { get; }

    public string Role { get; }
}

public class SelectUserAction
{
    public SelectUserAction(GetUserResponse user)
    {
        User = user;
    }

    public GetUserResponse User { get; }
}

public class DeleteUserAction
{
    public DeleteUserAction(GetUserResponse user)
    {
        User = user;
    }

    public GetUserResponse User { get; }
}

public class VerifingUserAction
{
    public VerifingUserAction(GetUserResponse user)
    {
        User = user;
    }

    public GetUserResponse User { get; }
}

public class VerifiedUserAction
{
    public VerifiedUserAction(GetUserResponse user)
    {
        User = user;
    }

    public GetUserResponse User { get; }
}
