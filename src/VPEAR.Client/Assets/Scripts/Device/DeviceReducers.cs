using Fluxor;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

#pragma warning disable IDE0060

public static partial class Reducers
{
    [ReducerMethod]
    public static DeviceListState ReduceFetchingDevicesAction(DeviceListState state, FetchingDevicesAction action)
    {
        return new DeviceListState(true, state.Devices, state.Status);
    }

    [ReducerMethod]
    public static DeviceListState ReduceFetchedDevicesAction(DeviceListState state, FetchedDevicesAction action)
    {
        return new DeviceListState(false, action.Devices, action.Status);
    }
}
