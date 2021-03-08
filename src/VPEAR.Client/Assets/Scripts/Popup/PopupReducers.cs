using Fluxor;

#pragma warning disable IDE0060

public static partial class Reducers
{
    [ReducerMethod]
    public static PopupState ReduceShowPopupAction(PopupState state, ShowPopupAction action)
    {
        return new PopupState(action.Title, action.Message, action.Action, true);
    }

    [ReducerMethod]
    public static PopupState ReduceClosePopupAction(PopupState state, ClosePopupAction action)
    {
        return new PopupState(state.Title, state.Message, state.Action, false);
    }
}
