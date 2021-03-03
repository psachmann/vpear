using Fluxor;
using UnityEngine;

public class NavigateToReducer : Reducer<NavigationState, NavigateToAction>
{
    public override NavigationState Reduce(NavigationState state, NavigateToAction action)
    {
        action.From?.Hide();
        action.To.Show();
        state.History.Push(action.To);

        return new NavigationState(action.To, state.History);
    }
}

public class NavigateBackReducer : Reducer<NavigationState, NavigateBackAction>
{
    public override NavigationState Reduce(NavigationState state, NavigateBackAction action)
    {
        if (state.History.Count == 1)
        {
            Application.Quit();
        }
        
        state.History.Pop().Hide();
        state.History.Peek().Show();

        return new NavigationState(state.History.Peek(), state.History);
    }
}
