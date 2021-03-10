using Fluxor;

#pragma warning disable IDE0060

public static partial class Reducers
{
    [ReducerMethod]
    public static LoginState ReduceLoginAction(LoginState state, LoginAction action)
    {
        return new LoginState(action.Name);
    }

    [ReducerMethod]
    public static LoginState ReduceLoginSucceededAction(LoginState state, LoginSucceededAction action)
    {
        return new LoginState(state.Name, true, action.IsAdmin);
    }

    [ReducerMethod]
    public static LoginState ReduceLogoutActin(LoginState state, LogoutAction action)
    {
        return new LoginState(string.Empty);
    }

    public static LoginState ReduceRegisterAction(LoginState state, RegisterAction action)
    {
        return new LoginState(action.Name);
    }
}
