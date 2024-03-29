using Fluxor;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;

public class LoginEffect : Effect<LoginAction>
{
    private readonly IVPEARClient _client;

    public LoginEffect(IVPEARClient client)
    {
        _client = client;
    }

    public override async Task HandleAsync(LoginAction action, IDispatcher dispatcher)
    {
        if (await _client.LoginAsync(action.Name, action.Password))
        {
            if (await _client.GetUsersAsync() != null)
            {
                dispatcher.Dispatch(new LoginSucceededAction(true));
            }
            else
            {
                dispatcher.Dispatch(new LoginSucceededAction(false));
            }

            dispatcher.Dispatch(new FetchingDevicesAction(null));
            dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName));
        }
        else
        {
            dispatcher.Dispatch(new LogoutAction());
            dispatcher.Dispatch(new ShowPopupAction("Login Error", _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class LogoutEffect : Effect<LogoutAction>
{
    private readonly IVPEARClient _client;
    private readonly NavigationService _navigationService;

    public LogoutEffect(IVPEARClient client, NavigationService navigationService)
    {
        _client = client;
        _navigationService = navigationService;
    }

    public override Task HandleAsync(LogoutAction action, IDispatcher dispatcher)
    {
        _client.Logout();
        _navigationService.ClearHistory();

        dispatcher.Dispatch(new NavigateToAction(Constants.LoginViewName));

        return Task.CompletedTask;
    }
}
