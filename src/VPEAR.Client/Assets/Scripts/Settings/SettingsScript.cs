using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : AbstractView
{
    [SerializeField] private InputField _deltaMinutesInput;
    [SerializeField] private Dropdown _colorScelDropDown;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _logoutButton;

    private IState<SettingsState> _settingsState;

    private void Start()
    {
        _settingsState = s_provider.GetRequiredService<IState<SettingsState>>();
        _settingsState.StateChanged += SettingsStateChanged;
        _deltaMinutesInput.contentType = InputField.ContentType.DecimalNumber;
        _applyButton.onClick.AddListener(OnApplyClick);
        _logoutButton.onClick.AddListener(OnLogoutAction);

        SettingsStateChanged(this, _settingsState.Value);
    }

    private void OnDestroy()
    {
        _settingsState.StateChanged -= SettingsStateChanged;
    }

    private void SettingsStateChanged(object sender, SettingsState state)
    {
        _deltaMinutesInput.text = state.DeltaMinutes.ToString();
        _colorScelDropDown.value = (int)state.ColorScale;
    }

    private void OnApplyClick()
    {
        _dispatcher.Dispatch(new ApplySettingsAction(float.Parse(_deltaMinutesInput.text), (ColorScale)_colorScelDropDown.value));
    }

    private void OnLogoutAction()
    {
        _dispatcher.Dispatch(new LogoutAction());
    }
}
