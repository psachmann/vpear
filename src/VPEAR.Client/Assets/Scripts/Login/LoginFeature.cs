using Fluxor;

public class LoginFeature : Feature<LoginState>
{
    public override string GetName()
    {
        return "Login";
    }

    protected override LoginState GetInitialState()
    {
        return new LoginState(string.Empty, string.Empty);
    }
}
