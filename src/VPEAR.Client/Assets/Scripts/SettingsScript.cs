using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Extensions;

public class SettingsScript : AbstractView
{
    [SerializeField] private Text counterText;
    [SerializeField] private Button incrementButton;
    [SerializeField] private Button decrmenntButton;

    private IState<CounterState> counterState;

    private void Start()
    {
        this.counterState = s_provider.GetRequiredService<IState<CounterState>>();
        this.counterState.StateChanged += this.CounterStateChanged;
        this.counterText.text = $"Counter: {this.counterState.Value.Counter}";
        this.incrementButton.onClick.AddListener(() => this._dispatcher.Dispatch(new IncrementCounterAction()));
        this.decrmenntButton.onClick.AddListener(() => this._dispatcher.Dispatch(new DecrementCounterAction()));
    }

    private void CounterStateChanged(object sender, CounterState state)
    {
        this.counterText.text = $"Counter: {state.Counter}";
    }
}
