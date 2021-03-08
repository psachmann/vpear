using Fluxor;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

public class FetchingDevicesEffect : Effect<FetchingDevicesAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public FetchingDevicesEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchingDevicesAction action, IDispatcher dispatcher)
    {
        var container = await _client.GetDevicesAsync(action.Status);

        if (container != null)
        {
            dispatcher.Dispatch(new FetchedDevicesAction(action.Status, container.Items));
        }
        else
        {
            _logger.Error(_client.Error.Message);
            dispatcher.Dispatch(new FetchedDevicesAction(action.Status, new List<GetDeviceResponse>()));
            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class DeviceSearchEffect : Effect<DeviceSearchAction>
{
    private readonly IVPEARClient _clien;
    private readonly ILogger _logger;

    public DeviceSearchEffect(IVPEARClient client, ILogger logger)
    {
        _clien = client;
        _logger = logger;
    }

    public override async Task HandleAsync(DeviceSearchAction action, IDispatcher dispatcher)
    {
        if (await _clien.PostDevicesAsync(action.Address, action.Mask))
        {
            dispatcher.Dispatch(new ShowPopupAction("Device Search", "The server will search for devices.",
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
        else
        {
            _logger.Error(_clien.ErrorMessage);

            dispatcher.Dispatch(new ShowPopupAction("Error", _clien.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}
