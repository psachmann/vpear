using System;
using VPEAR.Core.Wrappers;

public class UserDetailScript : AbstractView
{
    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        var args = eventArgs as NavigateEventArgs<GetUserResponse>;

        if (args != null && args.To == this)
        {
            this.Load(args.Payload);
        }
    }

    private void Load(GetUserResponse user)
    {
        Logger.Debug("Loading device information...");
    }
}
