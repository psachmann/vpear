using Fluxor;
using System.Collections.Generic;

public class NavigationFeature : Feature<NavigationState>
{
    public override string GetName()
    {
        return "Navigation";
    }

    protected override NavigationState GetInitialState()
    {
        return new NavigationState(null, new Stack<AbstractView>());
    }
}
