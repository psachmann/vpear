using Fluxor;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

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
