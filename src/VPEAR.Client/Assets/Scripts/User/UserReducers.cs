using Fluxor;

#pragma warning disable IDE0060

public static partial class Reducers
{
    [ReducerMethod]
    public static UserListState ReduceFetchingUsersAction(UserListState state, FetchingUsersAction action)
    {
        return new UserListState(true, state.Users, state.Role);
    }

    [ReducerMethod]
    public static UserListState ReduceFetchedUsersAction(UserListState state, FetchedUsersAction action)
    {
        return new UserListState(false, action.Users, action.Role);
    }

    [ReducerMethod]
    public static UserDetailState ReduceSelectUserAction(UserDetailState state, SelectUserAction action)
    {
        return new UserDetailState(false, action.User);
    }

    [ReducerMethod]
    public static UserDetailState ReduceUpdatingUserAction(UserDetailState state, UpdatingUserAction action)
    {
        return new UserDetailState(true, state.User);
    }

    [ReducerMethod]
    public static UserDetailState ReduceUpdatedUserAction(UserDetailState state, UpdatedUserAction action)
    {
        return new UserDetailState(false, action.User);
    }
}
