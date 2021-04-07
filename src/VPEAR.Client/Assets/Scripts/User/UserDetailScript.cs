using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class UserDetailScript : AbstractView
{
    [SerializeField] private Text _userIdText;
    [SerializeField] private Text _userNameText;
    [SerializeField] private Text _userRolesText;
    [SerializeField] private Toggle _userIsVerifiedToggle;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;

    private IState<UserDetailState> _userDetailState;
    private UserDetailState _userDetailStateValue;

    private void Start()
    {
        _userDetailState = s_provider.GetRequiredService<IState<UserDetailState>>();
        _userDetailState.StateChanged += UserDetailStateChanged;
        _userDetailStateValue = _userDetailState.Value;
        _saveButton.onClick.AddListener(OnSaveClick);
        _deleteButton.onClick.AddListener(OnDeleteClick);

        UserDetailStateChanged(this, _userDetailState.Value);
    }

    private void OnDestroy()
    {
        _userDetailState.StateChanged -= UserDetailStateChanged;
    }

    private void UserDetailStateChanged(object sender, UserDetailState state)
    {
        _userDetailStateValue = state;
        _userIdText.text = state.User.Id;
        _userNameText.text = state.User.Name;
        _userRolesText.text = string.Join(", ", state.User.Roles);
        _userIsVerifiedToggle.isOn = state.User.IsVerified;

        if (state.User.IsVerified)
        {
            _userIsVerifiedToggle.enabled = false;
        }
        else
        {
            _userIsVerifiedToggle.enabled = true;
        }
    }


    private void OnSaveClick()
    {
        _dispatcher.Dispatch(new VerifingUserAction(_userDetailStateValue.User));
    }

    private void OnDeleteClick()
    {
        _dispatcher.Dispatch(new DeleteUserAction(_userDetailState.Value.User));
    }
}
