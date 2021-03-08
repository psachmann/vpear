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
        if (true/*await _client.LoginAsync(action.Name, action.Password)*/)
        {
            dispatcher.Dispatch(new LoginSucceededAction());
            dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName));
        }
        else
        {
            dispatcher.Dispatch(new LogoutAction());
            dispatcher.Dispatch(new ShowPopupAction("Login Error", "Login Error Message",
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
        _navigationService.NavigateTo(Constants.LoginViewName);

        return Task.CompletedTask;
    }
}

public class RegisterEffect : Effect<RegisterAction>
{
    private readonly IVPEARClient _client;

    public RegisterEffect(IVPEARClient client)
    {
        _client = client;
    }

    public override async Task HandleAsync(RegisterAction action, IDispatcher dispatcher)
    {
        if (await _client.RegisterAsync(action.Name, action.Password))
        {
            dispatcher.Dispatch(new LogoutAction());
            dispatcher.Dispatch(new ShowPopupAction(Constants.RegisterTitleText, Constants.RegisterSucceededMessageText,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
        else
        {
            dispatcher.Dispatch(new LogoutAction());
            dispatcher.Dispatch(new ShowPopupAction(Constants.RegisterTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}
