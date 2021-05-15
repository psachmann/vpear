using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : AbstractView
{
    [SerializeField] private InputField _stepSizeInput;
    [SerializeField] private InputField _thresholdInput;
    [SerializeField] private InputField _deltaMinutesInput;
    [SerializeField] private InputField _oldPasswordInput;
    [SerializeField] private InputField _newPasswordInput;
    [SerializeField] private Dropdown _colorScelDropDown;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private Button _saveButton;

    private IState<ARState> _arState;
    private ARState _arStateValue;
    private IState<LoginState> _loginState;
    private LoginState _loginStateValue;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _loginState = s_provider.GetRequiredService<IState<LoginState>>();
        _loginState.StateChanged += LoginStateChanged;
        _stepSizeInput.contentType = InputField.ContentType.IntegerNumber;
        _thresholdInput.contentType = InputField.ContentType.DecimalNumber;
        _deltaMinutesInput.contentType = InputField.ContentType.DecimalNumber;
        _oldPasswordInput.contentType = InputField.ContentType.Password;
        _newPasswordInput.contentType = InputField.ContentType.Password;
        _saveButton.onClick.AddListener(OnSaveClick);
        _logoutButton.onClick.AddListener(OnLogoutClick);

        ARStateChanged(this, _arState.Value);
        LoginStateChanged(this, _loginState.Value);
    }

    private void OnDestroy()
    {
        _arState.StateChanged -= ARStateChanged;
        _loginState.StateChanged -= LoginStateChanged;
    }

    private void ARStateChanged(object sender, ARState state)
    {
        _arStateValue = state;
        _stepSizeInput.text = state.StepSize.ToString();
        _thresholdInput.text = state.Threshold.ToString();
        _deltaMinutesInput.text = state.DeltaMinutes.TotalMinutes.ToString();
        _colorScelDropDown.value = (int)state.ColorScale;
    }

    private void LoginStateChanged(object sender, LoginState state)
    {
        _loginStateValue = state;
    }

    private void OnSaveClick()
    {
        _dispatcher.Dispatch(new ApplySettingsAction(
            int.Parse(_stepSizeInput.text),
            float.Parse(_thresholdInput.text),
            float.Parse(_deltaMinutesInput.text),
            (ColorScale)_colorScelDropDown.value));

        if (!string.IsNullOrEmpty(_oldPasswordInput.text) || !string.IsNullOrEmpty(_newPasswordInput.text))
        {   
            _dispatcher.Dispatch(new ChangePasswordAction(_loginStateValue.Name, _oldPasswordInput.text, _newPasswordInput.text));
        }

        _oldPasswordInput.text = string.Empty;
        _newPasswordInput.text = string.Empty;
    }

    private void OnLogoutClick()
    {
        _dispatcher.Dispatch(new LogoutAction());
    }
}
