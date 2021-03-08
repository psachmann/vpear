using System.Collections.Generic;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class FetchingDevicesAction
{
    public FetchingDevicesAction(DeviceStatus? status)
    {
        Status = status;
    }

    public DeviceStatus? Status { get; }
}

public class FetchedDevicesAction
{
    public FetchedDevicesAction(DeviceStatus? status, IEnumerable<GetDeviceResponse> devices)
    {
        Status = status;
        Devices = devices;
    }

    public DeviceStatus? Status { get; }

    public IEnumerable<GetDeviceResponse> Devices { get; }
}

public class DeviceSearchAction
{
    public DeviceSearchAction(string address, string mask)
    {
        Address = address;
        Mask = mask;
    }

    public string Address { get; }

    public string Mask { get; }
}
