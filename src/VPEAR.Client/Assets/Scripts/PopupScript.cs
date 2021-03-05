using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupScript : AbstractView
{
    [SerializeField] private Button actionButton = null;
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text messageText = null;

    private void Start()
    {
        this._logger.Debug($"Initialized {this.GetType()}");
    }

    public void Clear()
    {
        this.actionButton.onClick.RemoveAllListeners();
        this.titleText.text = string.Empty;
        this.messageText.text = string.Empty;
    }

    public override void Hide()
    {
        base.Hide();
        this.Clear();
    }

    public void Show(string titleText, string messageText, UnityAction action)
    {
        this.actionButton.onClick.RemoveAllListeners();
        this.actionButton.onClick.AddListener(action);
        this.titleText.text = titleText;
        this.messageText.text = messageText;
        base.Show();
    }
}
