public class LoginAction
{
    public LoginAction(string name, string password, bool isSignedIn = default)
    {
        Name = name;
        Password = password;
        IsSignedIn = isSignedIn;
    }

    public bool IsSignedIn { get; }

    public string Name { get; }

    public string Password { get; }
}

public class LoginSucceededAction
{
    public LoginSucceededAction(bool isAdmin)
    {
        IsAdmin = isAdmin;
    }

    public bool IsAdmin { get; }
}

public class RegisterAction
{
    public RegisterAction(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; }

    public string Password { get; }
}

public class LogoutAction
{
}
