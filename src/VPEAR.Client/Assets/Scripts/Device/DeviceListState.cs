using System;
using System.Collections.Generic;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceListState
{
    public DeviceListState(bool isLoading, IEnumerable<GetDeviceResponse> devices, DeviceStatus? status)
    {
        IsLoading = isLoading;
        Devices = devices ?? throw new ArgumentNullException(nameof(devices));
        Status = status;
    }

    public bool IsLoading { get; }

    public DeviceStatus? Status { get; }

    public IEnumerable<GetDeviceResponse> Devices { get; }
}
