using Fluxor;
using System.Collections.Generic;

public class NavigationFeature : Feature<NavigationState>
{
    public override string GetName()
    {
        return nameof(NavigationState);
    }

    protected override NavigationState GetInitialState()
    {
        return new NavigationState(Constants.LoginViewName);
    }
}
