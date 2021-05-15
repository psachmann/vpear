using VPEAR.Core.Wrappers;

public class DeviceDetailState
{
    public DeviceDetailState(bool isLoading, GetDeviceResponse device, GetFiltersResponse filters)
    {
        IsLoading = isLoading;
        Device = device;
        Filters = filters;
    }

    public bool IsLoading { get; }

    public GetDeviceResponse Device { get; }

    public GetFiltersResponse Filters { get; }
}
