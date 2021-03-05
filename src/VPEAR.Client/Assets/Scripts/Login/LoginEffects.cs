using Fluxor;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;

public class LoginActionEffect : Effect<LoginAction>
{
    private readonly IVPEARClient _client;

    public LoginActionEffect(IVPEARClient client)
    {
        _client = client;
    }

    public override async Task HandleAsync(LoginAction action, IDispatcher dispatcher)
    {
        if (true/*await _client.LoginAsync(action.Name, action.Password)*/)
        {
            dispatcher.Dispatch(new LoginSuccessAction(action.Name));
            dispatcher.Dispatch(new NavigateToAction(Constants.DeviceListViewName));
        }
        else
        {
            dispatcher.Dispatch(new LoginErrorAction(Constants.LoginFailedTitleText, _client.ErrorMessage));
        }
    }
}
