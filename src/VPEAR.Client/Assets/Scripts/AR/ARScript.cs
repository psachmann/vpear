using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Plane _anchorPlane;

    private const int c_x = 16;
    private const int c_y = 1;
    private const int c_z = 16;
    private IState<ARState> _arState;
    private IState<SettingsState> _settingsState;
    private Plane[,] _plane = new Plane[c_x, c_z];

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _backButton.onClick.AddListener(OnBackClick);

        Initialize();
        ARStateChanged(this, _arState.Value);
    }

    private void ARStateChanged(object sender, ARState state)
    {
    }

    private void OnBackClick()
    {
        _dispatcher.Dispatch(new ChangeSceneAction(Constants.MenuSceneName));
    }

    private void Initialize()
    {

    }
}
