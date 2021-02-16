using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class LoginScript : AbstractView
{
    private Button loginButton = null;
    private InputField userNameInput = null;
    private InputField userPasswordInput = null;

    private void Start()
    {
        this.loginButton = this.GetComponentInChildren<Button>();
        this.loginButton.onClick.AddListener(LoginUser);

        var inputs = new List<InputField>();
        this.GetComponentsInChildren(inputs);
        this.userNameInput = inputs.First(input => string.Equals(input.name, Constants.UserNameInputName));
        this.userNameInput.onValueChanged.AddListener(IsLoginEnabled);
        this.userPasswordInput = inputs.First(input => string.Equals(input.name, Constants.UserPasswordInputName));
        this.userPasswordInput.contentType = InputField.ContentType.Password;
        this.userPasswordInput.onValueChanged.AddListener(IsLoginEnabled);
        this.IsLoginEnabled();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void LoginUser()
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
