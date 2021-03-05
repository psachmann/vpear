public class LoginState
{
    public LoginState(string name, bool isSignedIn = default)
    {
        this.Name = name;
        this.IsSignedIn = isSignedIn;
    }

    public string Name { get; }

    public bool IsSignedIn { get; }
}
