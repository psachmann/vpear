using Fluxor;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class DeviceDetailFeature : Feature<DeviceDetailState>
{
    public override string GetName()
    {
        return nameof(DeviceDetailState);
    }

    protected override DeviceDetailState GetInitialState()
    {
        var device = new GetDeviceResponse()
        {
            Address = string.Empty,
            DisplayName = string.Empty,
            Frequency = default,
            Id = string.Empty,
            RequiredSensors = default,
            Status = default,
        };
        var filters = new GetFiltersResponse();

        return new DeviceDetailState(false, device, filters);
    }
}

public class DeviceListFeature : Feature<DeviceListState>
{
    public override string GetName()
    {
        return nameof(DeviceListState);
    }

    protected override DeviceListState GetInitialState()
    {
        return new DeviceListState(false, new List<GetDeviceResponse>(), null);
    }
}
