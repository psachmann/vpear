using System;
using VPEAR.Core.Wrappers;

public class DeviceDetailState
{
    public DeviceDetailState(bool isLoading, GetDeviceResponse device, GetFiltersResponse filters)
    {
        IsLoading = isLoading;
        Device = device ?? throw new ArgumentNullException(nameof(device));
        Filters = filters ?? throw new ArgumentNullException(nameof(filters));
    }

    public bool IsLoading { get; }

    public GetDeviceResponse Device { get; }

    public GetFiltersResponse Filters { get; }
}
