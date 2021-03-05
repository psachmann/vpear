using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : AbstractView
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private InputField _userNameInput;
    [SerializeField] private InputField _userPasswordInput;

    private IState<LoginState> _loginState;

    private void Start()
    {
        _loginState = s_provider.GetRequiredService<IState<LoginState>>();
        _loginState.StateChanged += LoginStateChanged;
        _loginButton.onClick.AddListener(() => _dispatcher.Dispatch(new LoginAction(_userNameInput.text, _userPasswordInput.text)));
        _userNameInput.onValueChanged.AddListener((_) => IsLoginEnabled());
        _userPasswordInput.contentType = InputField.ContentType.Password;
        _userPasswordInput.onValueChanged.AddListener((_) => IsLoginEnabled());
        IsLoginEnabled();
    }

    private void LoginStateChanged(object sender, LoginState state)
    {
        _userNameInput.text = state.Name;
        _userPasswordInput.text = string.Empty;
    }

    private void IsLoginEnabled(string _ = default)
    {
        if (string.IsNullOrEmpty(_userNameInput.text)
            || string.IsNullOrEmpty(_userPasswordInput.text))
        {
            _loginButton.enabled = false;
        }
        else
        {
            _loginButton.enabled = true;
        }
    }
}
