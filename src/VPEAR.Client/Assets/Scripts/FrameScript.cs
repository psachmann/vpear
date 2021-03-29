using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class FrameScript : AbstractBase
{
    [SerializeField] private CanvasGroup _navigationPanel;
    [SerializeField] private Button _devices;
    [SerializeField] private Button _users;
    [SerializeField] private Button _settings;

    private IState<NavigationState> _navigationState;
    private IState<LoginState> _loginState;
    private LoginState _loginStateValue;

    private void Start()
    {
        _navigationState = s_provider.GetRequiredService<IState<NavigationState>>();
        _navigationState.StateChanged += NavivigationStateChanged;
        _loginState = s_provider.GetRequiredService<IState<LoginState>>();
        _loginState.StateChanged += LoginStateChanged;
        _loginStateValue = _loginState.Value;
        _devices.onClick.AddListener(OnDevicesClick);
        _users.onClick.AddListener(OnUsersClick);
        _settings.onClick.AddListener(OnSettingsClick);

        if (!_loginState.Value.IsSignedIn)
        {
            _dispatcher.Dispatch(new NavigateToAction(Constants.LoginViewName));
        }

        NavivigationStateChanged(this, _navigationState.Value);
        LoginStateChanged(this, _loginState.Value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _dispatcher.Dispatch(new NavigateBackAction());
        }
    }

    private void OnDestroy()
    {
        _navigationState.StateChanged -= NavivigationStateChanged;
        _loginState.StateChanged -= LoginStateChanged;
    }

    private void NavivigationStateChanged(object sender, NavigationState state)
    {
        if (string.Equals(state.ViewName, Constants.LoginViewName))
        {
            HideContent();
        }
        else
        {
            ShowContent();
        }
    }

    private void LoginStateChanged(object sender, LoginState state)
    {
        _loginStateValue = state;
    }

    private void OnDevicesClick()
    {
        _dispatcher.Dispatch(new FetchingDevicesAction(null));
        _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName));
    }

    private void OnUsersClick()
    {
        _dispatcher.Dispatch(new FetchingUsersAction(null));
        _dispatcher.Dispatch(new NavigateToAction(Constants.UserListViewName));
    }

    private void OnSettingsClick()
    {
        _dispatcher.Dispatch(new NavigateToAction(Constants.SettingsViewName));
    }

    private void HideContent()
    {
        _navigationPanel.gameObject.SetActive(false);
    }

    private void ShowContent()
    {
        _navigationPanel.gameObject.SetActive(true);

        if (!_loginStateValue.IsAdmin)
        {
            _users.gameObject.SetActive(false);
        }
    }
}
