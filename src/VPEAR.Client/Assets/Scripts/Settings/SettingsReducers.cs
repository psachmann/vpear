using Fluxor;

public static partial class Reducers
{
    [ReducerMethod]
    public static SettingsState ReduceApplySettingsAction(SettingsState state, ApplySettingsAction action)
    {
        return new SettingsState(action.StepSize, action.DeltaMinutes, action.ColorScale);
    }
}
