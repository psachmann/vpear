using Fluxor;
using Serilog;

#pragma warning disable IDE0060

public static partial class Reducers
{
    [ReducerMethod]
    public static NavigationState ReduceNavigateTo(NavigationState state, NavigateToAction action)
    {
        return new NavigationState(action.NextView);
    }
}
