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
        _loginButton.onClick.AddListener(OnLoginClick);
        _userNameInput.onValueChanged.AddListener(IsLoginEnabled);
        _userPasswordInput.contentType = InputField.ContentType.Password;
        _userPasswordInput.onValueChanged.AddListener(IsLoginEnabled);

        IsLoginEnabled();
    }

    private void OnDestroy()
    {
        _loginState.StateChanged -= LoginStateChanged;
    }

    private void LoginStateChanged(object sender, LoginState state)
    {
        _userNameInput.text = state.Name;
        _userPasswordInput.text = string.Empty;
    }

    private void IsLoginEnabled(string _ = default)
    {
        if (string.IsNullOrEmpty(_userNameInput.text) || string.IsNullOrEmpty(_userPasswordInput.text))
        {
            _loginButton.enabled = false;
        }
        else
        {
            _loginButton.enabled = true;
        }
    }

    private void OnLoginClick()
    {
        _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName));
        // _dispatcher.Dispatch(new LoginAction(_userNameInput.text, _userPasswordInput.text));
    }
}
