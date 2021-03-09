using Fluxor;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

public class FetchingUsersEffect : Effect<FetchingUsersAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public FetchingUsersEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchingUsersAction action, IDispatcher dispatcher)
    {
        var container = await _client.GetUsersAsync(action.Role);

        if (container != null)
        {
            dispatcher.Dispatch(new FetchedUsersAction(container.Items, action.Role));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new FetchedUsersAction(new List<GetUserResponse>(), null));
            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class UpdatingUserEffect : Effect<UpdatingUserAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public UpdatingUserEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(UpdatingUserAction action, IDispatcher dispatcher)
    {
        if (await _client.PutUserAsync(action.User.Name, action.OldPassword, action.NewPassword, action.IsVerfied))
        {
            action.User.IsVerified = action.IsVerfied;
            dispatcher.Dispatch(new UpdatedUserAction(action.User));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new UpdatedUserAction(action.User));
            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class DeleteUserEffect : Effect<DeleteUserAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public DeleteUserEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(DeleteUserAction action, IDispatcher dispatcher)
    {
        if (await _client.DeleteUserAsync(action.User.Name))
        {
            dispatcher.Dispatch(new NavigateBackAction());
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}
