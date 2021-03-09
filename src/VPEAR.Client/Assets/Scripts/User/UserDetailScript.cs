using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class UserDetailScript : AbstractView
{
    [SerializeField] private Text _userIdText;
    [SerializeField] private Text _userNameText;
    [SerializeField] private Text _userRolesText;
    [SerializeField] private Toggle _userIsVerifiedToggle;
    [SerializeField] private InputField _oldPasswordInput;
    [SerializeField] private InputField _newPasswordInput;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;

    private IState<UserDetailState> _userDetailState;

    private void Start()
    {
        _userDetailState = s_provider.GetRequiredService<IState<UserDetailState>>();
        _userDetailState.StateChanged += UserDetailStateChanged;
        _oldPasswordInput.contentType = InputField.ContentType.Password;
        _newPasswordInput.contentType = InputField.ContentType.Password;
        _saveButton.onClick.AddListener(OnSaveClick);
        _deleteButton.onClick.AddListener(OnDeleteClick);

        UserDetailStateChanged(this, _userDetailState.Value);
    }

    private void UserDetailStateChanged(object sender, UserDetailState state)
    {
        _userIdText.text = state.User.Id;
        _userNameText.text = state.User.Name;
        _userRolesText.text = string.Join(", ", state.User.Roles);
        _userIsVerifiedToggle.isOn = state.User.IsVerified;
    }


    private void OnSaveClick()
    {
        if (string.IsNullOrEmpty(_oldPasswordInput.text) || string.IsNullOrEmpty(_newPasswordInput.text))
        {
            _dispatcher.Dispatch(new UpdatingUserAction(_userDetailState.Value.User, _userIsVerifiedToggle.isOn));
        }
        else
        {
            _dispatcher.Dispatch(new UpdatingUserAction(_userDetailState.Value.User, _userIsVerifiedToggle.isOn, _oldPasswordInput.text, _newPasswordInput.text));
        }

        _oldPasswordInput.text = string.Empty;
        _newPasswordInput.text = string.Empty;
    }

    private void OnDeleteClick()
    {
        _dispatcher.Dispatch(new DeleteUserAction(_userDetailState.Value.User));
    }
}
