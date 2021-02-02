using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : AbstractView
{
    private InputField userNameInput = null;
    private InputField userPasswordInput = null;

    private void Start()
    {
        var loginButton = this.GetComponentInChildren<Button>();
        loginButton.onClick.AddListener(LoginUser);

        var inputs = new List<InputField>();
        this.GetComponentsInChildren(inputs);
        this.userNameInput = inputs.First(input => string.Equals(input.name, Constants.UserNameInputName));
        this.userPasswordInput = inputs.First(input => string.Equals(input.name, Constants.UserPasswordInputName));
    }

    private void LoginUser()
    {
        var view = this.viewService.GetViewByName(Constants.DeviceListViewName);

        Debug.Log("Go to 'DeviceListView'");

        // client.LoginAsync(this.userNameInput.text, this.userPasswordInput.text);

        this.viewService.GoTo(view);
    }
}
