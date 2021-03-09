using Fluxor;

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

    [ReducerMethod]
    public static DeviceDetailState ReduceFetchingDeviceAction(DeviceDetailState state, FetchingDeviceAction action)
    {
        return new DeviceDetailState(true, state.Device, state.Filters);
    }

    [ReducerMethod]
    public static DeviceDetailState ReduceFetchedDeviceAction(DeviceDetailState state, FetchedDeviceAction action)
    {
        return new DeviceDetailState(false, action.Device, action.Filters);
    }

    [ReducerMethod]
    public static DeviceDetailState ReduceUpdatingDeviceAction(DeviceDetailState state, UpdatingDeviceAction action)
    {
        return new DeviceDetailState(true, state.Device, state.Filters);
    }

    [ReducerMethod]
    public static DeviceDetailState ReduceFetchedDeviceAction(DeviceDetailState state, UpdatedDeviceAction action)
    {
        return new DeviceDetailState(false, action.Device, action.Filters);
    }

    [ReducerMethod]
    public static DeviceDetailState ReduceSelectDeviceAction(DeviceDetailState state, SelectDeviceAction action)
    {
        return new DeviceDetailState(false, action.Device, state.Filters);
    }
}
