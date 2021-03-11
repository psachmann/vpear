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

public class UpdatingUserAction
{
    public UpdatingUserAction(GetUserResponse user, bool isVerified, string oldPassword = default, string newPassword = default)
    {
        User = user;
        IsVerfied = isVerified;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    public GetUserResponse User { get; }

    public bool IsVerfied { get; }

    public string OldPassword { get; }

    public string NewPassword { get; }
}

public class UpdatedUserAction
{
    public UpdatedUserAction(GetUserResponse user)
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
