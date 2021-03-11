using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class UserListState
{
    public UserListState(bool isLoading, IEnumerable<GetUserResponse> users = default, string role = default)
    {
        IsLoading = isLoading;
        Users = users;
        Role = role;
    }

    public bool IsLoading { get; }

    public IEnumerable<GetUserResponse> Users { get; }

    public string Role { get; }
}
