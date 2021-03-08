using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : AbstractView
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _messageText;

    private IState<PopupState> _popupState;

    private void Start()
    {
        _popupState = s_provider.GetRequiredService<IState<PopupState>>();
        _popupState.StateChanged += PopupStateChanged;
        _logger.Debug($"Initialized {this.GetType()}");

        PopupStateChanged(this, _popupState.Value);
    }

    private void PopupStateChanged(object sender, PopupState state)
    {
        if (state.IsVisible)
        {
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(state.Action);
            _titleText.text = state.Title;
            _messageText.text = state.Message;

            Show();
        }
        else
        {
            Hide();
        }
    }
}
