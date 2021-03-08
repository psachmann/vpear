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
        _devices.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName)));
        _users.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.UserListViewName)));
        _settings.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.SettingsViewName)));
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
