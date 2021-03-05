using Fluxor;

public static partial class Reducers
{
    [ReducerMethod]
    public static LoginState ReduceLoginAction(LoginState state, LoginAction action)
    {
        return new LoginState(action.Name);
    }

    [ReducerMethod]
    public static LoginState ReduceLoginErrorAction(LoginState state, LoginErrorAction action)
    {
        return new LoginState(string.Empty);
    }

    [ReducerMethod]
    public static LoginState ReduceLoginSuccessAction(LoginState state, LoginSuccessAction action)
    {
        return new LoginState(action.Name, true);
    }
}
