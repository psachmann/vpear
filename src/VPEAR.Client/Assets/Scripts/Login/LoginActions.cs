public class LoginAction
{
    public LoginAction(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; }

    public string Password { get; }
}

public class LoginErrorAction : ErrorAction
{
    public LoginErrorAction(string title, string message)
    {
        Title = title;
        Message = message;
        Action = default;
    }
}

public class LoginSuccessAction
{
    public LoginSuccessAction(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
