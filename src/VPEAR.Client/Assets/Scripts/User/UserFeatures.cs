using Fluxor;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class UserDetailFeature : Feature<UserDetailState>
{
    public override string GetName()
    {
        return nameof(UserDetailState);
    }

    protected override UserDetailState GetInitialState()
    {
        var user = new GetUserResponse()
        {
            Id = string.Empty,
            IsVerified = false,
            Name = string.Empty,
            Roles = new List<string>(),
        };

        return new UserDetailState(false, user);
    }
}

public class UserListFeature : Feature<UserListState>
{
    public override string GetName()
    {
        return nameof(UserListState);
    }

    protected override UserListState GetInitialState()
    {
        return new UserListState(false, new List<GetUserResponse>(), string.Empty);
    }
}
