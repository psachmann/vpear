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

    private void Start()
    {
        _navigationState = s_provider.GetRequiredService<IState<NavigationState>>();
        _navigationState.StateChanged += NavivigationStateChanged;
        _devices.onClick.AddListener(OnDevicesClick);
        _users.onClick.AddListener(OnUsersClick);
        _settings.onClick.AddListener(OnSettingsClick);
        _dispatcher.Dispatch(new NavigateToAction(Constants.LoginViewName));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _dispatcher.Dispatch(new NavigateBackAction());
        }
    }

    private void NavivigationStateChanged(object sender, NavigationState state)
    {
        if (state.ViewName != Constants.LoginViewName)
        {
            ShowContent();
        }
        else
        {
            HideContent();
        }
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
        _navigationPanel.alpha = 0;
        _navigationPanel.interactable = false;
    }

    private void ShowContent()
    {
        _navigationPanel.interactable = true;
        _navigationPanel.alpha = 1;
    }
}
