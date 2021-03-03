using Fluxor;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : AbstractView
{
    private IState<LoginState> state;

    #region Unity

    [SerializeField] private Button loginButton = null;
    [SerializeField] private InputField userNameInput = null;
    [SerializeField] private InputField userPasswordInput = null;

    private void Start()
    {
        this.state = null;
        this.loginButton.onClick.AddListener(() => this.OnLoginClick());
        this.userNameInput.onValueChanged.AddListener((_) => this.IsLoginEnabled());
        this.userPasswordInput.contentType = InputField.ContentType.Password;
        this.userPasswordInput.onValueChanged.AddListener((_) => this.IsLoginEnabled());
        this.IsLoginEnabled();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    #endregion Unity

    private void OnLoginClick()
    {
        var popup = (PopupScript)this.viewService.GetViewByName(Constants.PopupViewName);

        this.viewService.ShowContent();
        this.viewService.GoTo(Constants.DeviceListViewName);
/*
        if (await Client.LoginAsync(this.userNameInput.text, this.userPasswordInput.text))
        {
            this.viewService.GoTo(Constants.DeviceListViewName);
        }
        else
        {
            Logger.Debug("{@Error}", Client.Error);
            popup.Show(Constants.LoginFailedTitleText, Client.ErrorMessage, LoginFailureAction);
        }
*/
    }

    private void LoginFailureAction()
    {
        var popup = (PopupScript)this.viewService.GetViewByName(Constants.PopupViewName);

        popup.Hide();
        this.userNameInput.text = string.Empty;
        this.userPasswordInput.text = string.Empty;
    }

    private void IsLoginEnabled(string _ = default)
    {
        if (string.IsNullOrEmpty(this.userNameInput.text) || string.IsNullOrEmpty(this.userPasswordInput.text))
        {
            this.loginButton.enabled = false;
        }
        else
        {
            this.loginButton.enabled = true;
        }
    }
}
