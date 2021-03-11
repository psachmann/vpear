using Fluxor;

public class LoginFeature : Feature<LoginState>
{
    public override string GetName()
    {
        return nameof(LoginState);
    }

    protected override LoginState GetInitialState()
    {
        return new LoginState(string.Empty);
    }
}
