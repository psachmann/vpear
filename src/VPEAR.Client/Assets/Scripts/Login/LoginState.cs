public class LoginState
{
    public LoginState(string name, bool isSignedIn = default, bool isAdmin = default)
    {
        Name = name;
        IsAdmin = isAdmin;
        IsSignedIn = isSignedIn;
    }

    public string Name { get; }

    public bool IsAdmin { get; }

    public bool IsSignedIn { get; }
}
