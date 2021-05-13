using Fluxor;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
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

public class FetchingDeviceEffect : Effect<FetchingDeviceAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public FetchingDeviceEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchingDeviceAction action, IDispatcher dispatcher)
    {
        var filters = await _client.GetFiltersAsync(action.Device.Id);

        if (filters != null)
        {
            dispatcher.Dispatch(new FetchedDeviceAction(action.Device, filters));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class UpdatingDeviceEffect : Effect<UpdatingDeviceAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public UpdatingDeviceEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(UpdatingDeviceAction action, IDispatcher dispatcher)
    {
        if (await _client.PutFiltersAsync(action.Device.Id, action.Spot, action.Smooth, action.Noise)
            && await _client.PutDeviceAsync(action.Device.Id, action.Name, action.Frequnecy, null, action.Status))
        {
            var device = new GetDeviceResponse()
            {
                Address = action.Device.Address,
                DisplayName = action.Name,
                Frequency = action.Frequnecy,
                Id = action.Device.Id,
                RequiredSensors = action.Device.RequiredSensors,
                Status = action.Status,
            };
            var filters = new GetFiltersResponse()
            {
                Noise = action.Noise,
                Smooth = action.Smooth,
                Spot = action.Spot,
            };

            dispatcher.Dispatch(new UpdatedDeviceAction(device, filters));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new UpdatedDeviceAction(action.Device, action.Filters));
            dispatcher.Dispatch(new ShowPopupAction("Error", _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class DeleteDeviceEffect : Effect<DeleteDeviceAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public DeleteDeviceEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(DeleteDeviceAction action, IDispatcher dispatcher)
    {
        if (await _client.DeleteDeviceAsync(action.Device.Id))
        {
            dispatcher.Dispatch(new NavigateBackAction());
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new ShowPopupAction("Error", _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}

public class DeviceSearchEffect : Effect<DeviceSearchAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public DeviceSearchEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(DeviceSearchAction action, IDispatcher dispatcher)
    {
        if (await _client.PostDevicesAsync(action.Address, action.Mask))
        {
            dispatcher.Dispatch(new ShowPopupAction("Device Search", "The server will search for devices.",
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);

            dispatcher.Dispatch(new ShowPopupAction("Error", _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }
}
