public class LoginState
{
    public LoginState(string name, string password, bool isLoggedIn = default)
    {
        this.IsLoggedIn = isLoggedIn;
        this.Name = name;
        this.Password = password;
    }

    public bool IsLoggedIn { get; }

    public string Name { get; }

    public string Password { get; }
}
