using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : AbstractView
{
    [SerializeField] private InputField _stepSizeInput;
    [SerializeField] private InputField _deltaMinutesInput;
    [SerializeField] private Dropdown _colorScelDropDown;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _logoutButton;

    private IState<ARState> _arState;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _stepSizeInput.contentType = InputField.ContentType.IntegerNumber;
        _deltaMinutesInput.contentType = InputField.ContentType.DecimalNumber;
        _applyButton.onClick.AddListener(OnApplyClick);
        _logoutButton.onClick.AddListener(OnLogoutAction);

        ARStateChanged(this, _arState.Value);
    }

    private void OnDestroy()
    {
        _arState.StateChanged -= ARStateChanged;
    }

    private void ARStateChanged(object sender, ARState state)
    {
        _stepSizeInput.text = state.StepSize.ToString();
        _deltaMinutesInput.text = state.DeltaMinutes.ToString();
        _colorScelDropDown.value = (int)state.ColorScale;
    }

    private void OnApplyClick()
    {
        _dispatcher.Dispatch(new ApplySettingsAction(int.Parse(_stepSizeInput.text),float.Parse(_deltaMinutesInput.text), (ColorScale)_colorScelDropDown.value));
    }

    private void OnLogoutAction()
    {
        _dispatcher.Dispatch(new LogoutAction());
    }
}
