using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupScript : AbstractView
{
    private Button actionButton = null;
    private Text titleText = null;
    private Text messageText = null;

    private void Start()
    {
        var buttons = this.GetComponentsInChildren<Button>();
        this.actionButton = buttons.First(button => string.Equals(button.name, Constants.PopupActionButtonName));

        var texts = this.GetComponentsInChildren<Text>();
        this.titleText = texts.First(text => string.Equals(text.name, Constants.PopupTitleTextName));
        this.messageText = texts.First(text => string.Equals(text.name, Constants.PopupMessageTextName));

        Logger.Debug($"Initialized {this.GetType()}");
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
