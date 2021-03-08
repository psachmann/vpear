using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class UserListScript : AbstractView
{
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _itemTemplate;

    private IState<UserListState> _userListState;

    private void Start()
    {
        _userListState = s_provider.GetRequiredService<IState<UserListState>>();
        _userListState.StateChanged += UserListStateChanged;
        _itemTemplate.gameObject.SetActive(false);

        UserListStateChanged(this, _userListState.Value);
    }

    private void UserListStateChanged(object sender, UserListState state)
    {
        foreach (var button in _content.GetComponentsInChildren<Button>())
        {
            Destroy(button);
        }

        foreach (var user in state.Users)
        {
            var temp = Instantiate(_itemTemplate, _content.transform);

            temp.gameObject.SetActive(true);
            temp.GetComponentInChildren<Text>().text = $"Name: {user.Name}\tRoles: {string.Join(", ", user.Roles)}";
            temp.onClick.AddListener(() =>
            {
                _dispatcher.Dispatch(new SelectUserAction(user));
                _dispatcher.Dispatch(new NavigateToAction(Constants.UserDetailViewName));
            });
        }
    }

    public override void Show()
    {
        _dispatcher.Dispatch(new FetchingUsersAction(_userListState.Value.Role));

        base.Show();
    }
}
