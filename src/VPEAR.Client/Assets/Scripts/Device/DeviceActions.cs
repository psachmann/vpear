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

public class FetchingDeviceAction
{
    public FetchingDeviceAction(GetDeviceResponse device)
    {
        Device = device;
    }

    public GetDeviceResponse Device { get; }
}

public class FetchedDeviceAction
{
    public FetchedDeviceAction(GetDeviceResponse device, GetFiltersResponse filters)
    {
        Device = device;
        Filters = filters;
    }

    public GetDeviceResponse Device { get; }

    public GetFiltersResponse Filters { get; }
}

public class UpdatingDeviceAction
{
    public UpdatingDeviceAction(
        GetDeviceResponse device,
        GetFiltersResponse filters,
        string name,
        int frequnecy,
        DeviceStatus status,
        bool spot,
        bool smooth,
        bool noise)
    {
        Device = device;
        Filters = filters;
        Name = name;
        Frequnecy = frequnecy;
        Status = status;
        Spot = spot;
        Smooth = smooth;
        Noise = noise;
    }

    public GetDeviceResponse Device { get; }

    public GetFiltersResponse Filters { get; }

    public string Name { get; }

    public int Frequnecy { get; }

    public DeviceStatus Status { get; }

    public bool Spot { get; }

    public bool Smooth { get; }

    public bool Noise { get; }
}

public class UpdatedDeviceAction
{
    public UpdatedDeviceAction(GetDeviceResponse device, GetFiltersResponse filters)
    {
        Device = device;
        Filters = filters;
    }

    public GetDeviceResponse Device { get; }

    public GetFiltersResponse Filters { get; }
}

public class SelectDeviceAction
{
    public SelectDeviceAction(GetDeviceResponse device)
    {
        Device = device;
    }

    public GetDeviceResponse Device { get; }
}

public class DeleteDeviceAction
{
    public DeleteDeviceAction(GetDeviceResponse device)
    {
        Device = device;
    }

    public GetDeviceResponse Device { get; }
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
