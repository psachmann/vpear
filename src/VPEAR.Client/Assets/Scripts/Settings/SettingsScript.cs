using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : AbstractView
{
    [SerializeField] private Text counterText;
    [SerializeField] private Button incrementButton;
    [SerializeField] private Button decrmenntButton;
    [SerializeField] private Button _logoutButton;

    private IState<CounterState> counterState;

    private void Start()
    {
        counterState = s_provider.GetRequiredService<IState<CounterState>>();
        counterState.StateChanged += this.CounterStateChanged;
        incrementButton.onClick.AddListener(() => _dispatcher.Dispatch(new IncrementCounterAction()));
        decrmenntButton.onClick.AddListener(() => _dispatcher.Dispatch(new ChangeSceneAction(Constants.ARSceneId)));
        // _logoutButton.onClick.AddListener(() => _dispatcher.Dispatch(new LogoutAction()));

        CounterStateChanged(this, counterState.Value);
    }

    private void CounterStateChanged(object sender, CounterState state)
    {
        this.counterText.text = $"Counter: {state.Counter}";
    }
}
