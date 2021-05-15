using Fluxor;

public static partial class Reducers
{
    [ReducerMethod]
    public static NavigationState ReduceNavigateTo(NavigationState state, NavigateToAction action)
    {
        return new NavigationState(action.NextView);
    }
}
