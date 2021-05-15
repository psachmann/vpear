using Fluxor;
using Serilog;
using System;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;

namespace Assets.Scripts.Settings
{
    public class ChangePasswordEffect : Effect<ChangePasswordAction>
    {
        private readonly IVPEARClient _client;
        private readonly ILogger _logger;

        public ChangePasswordEffect(IVPEARClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public override async Task HandleAsync(ChangePasswordAction action, IDispatcher dispatcher)
        {
            if (await _client.PutPasswordAsync(action.UserName, action.OldPassword, action.NewPassword))
            {
                dispatcher.Dispatch(new ShowPopupAction("Information", "Password change was successful. When you close the popup you have to login with your new password.", () =>
                {
                    dispatcher.Dispatch(new ClosePopupAction());
                    dispatcher.Dispatch(new LogoutAction());
                }));
            }
            else
            {
                {
                    _logger.Error(_client.ErrorMessage);

                    dispatcher.Dispatch(new ShowPopupAction("Error", _client.ErrorMessage,
                        () => dispatcher.Dispatch(new ClosePopupAction())));
                }
            }
        }
    }
}
