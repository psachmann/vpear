using Fluxor;

public class LoginReducer : Reducer<LoginState, LoginAction>
{
    public override LoginState Reduce(LoginState state, LoginAction action)
    {
        throw new System.NotImplementedException();
    }
}

public class LoginFailedReducer : Reducer<LoginState, LoginFailedAction>
{
    public override LoginState Reduce(LoginState state, LoginFailedAction action)
    {
        throw new System.NotImplementedException();
    }
}

public class LoginSuccessReducer : Reducer<LoginState, LoginSuccessAction>
{
    public override LoginState Reduce(LoginState state, LoginSuccessAction action)
    {
        throw new System.NotImplementedException();
    }
}
