using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class UserDetailScript : AbstractView
{
    private GetUserResponse currentUser = null;

    #region Unity

    [SerializeField] private Text userIdText = null;
    [SerializeField] private Text userNameText = null;
    [SerializeField] private Text userRolesText = null;
    [SerializeField] private Toggle userIsVerifiedToggle = null;
    [SerializeField] private InputField oldPasswordInput = null;
    [SerializeField] private InputField newPasswordInput = null;
    [SerializeField] private Button saveButton = null;

    private void Start()
    {
        this.oldPasswordInput.contentType = InputField.ContentType.Password;
        this.newPasswordInput.contentType = InputField.ContentType.Password;
        this.saveButton.onClick.AddListener(() => this.OnSaveClick());
    }

    #endregion Unity

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        if (eventArgs is NavigateEventArgs<GetUserResponse> args && args.To == this)
        {
            this.Load(args.Payload);
        }
    }

    private void Load(GetUserResponse user)
    {
        this.currentUser = user;
        this.userIdText.text = user.Id;
        this.userNameText.text = user.Name;
        this.userRolesText.text = string.Join(", ", user.Roles);
        this.userIsVerifiedToggle.isOn = user.IsVerified;
    }

    private async void OnSaveClick()
    {
        if (this.currentUser.IsVerified != this.userIsVerifiedToggle.isOn
            && !await Client.PutUserAsync(this.userNameText.text, isVerified: this.userIsVerifiedToggle.isOn))
        {
            Helpers.ShowClientError(Client, this.viewService, () => this.OnSaveFailure());

            return;
        }

        if (!string.IsNullOrEmpty(this.oldPasswordInput.text)
            && !string.IsNullOrEmpty(this.newPasswordInput.text)
            && !await Client.PutUserAsync(this.userNameText.text, this.oldPasswordInput.text, this.newPasswordInput.text))
        {
            Helpers.ShowClientError(Client, this.viewService, () => this.OnSaveFailure());

            return;
        }
    }

    private void OnSaveFailure()
    {
        this.userIsVerifiedToggle.isOn = this.currentUser.IsVerified;
        this.oldPasswordInput.text = string.Empty;
        this.newPasswordInput.text = string.Empty;
    }
}
