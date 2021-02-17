using System;
using VPEAR.Core.Wrappers;

public class DeviceDetailScript : AbstractView
{
    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        var args = eventArgs as NavigateEventArgs<GetDeviceResponse>;

        if (args != null && args.To == this)
        {
            this.Load(args.Payload);
        }
    }

    private void Load(GetDeviceResponse device)
    {
        Logger.Debug("Loading device information...");
    }
}
