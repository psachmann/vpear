using Fluxor;
using System.Linq;

public static partial class Reducers
{
    [ReducerMethod]
    public static ARState ReduceFetchingFramesAction(ARState state, FetchingFramesAction action)
    {
        return new ARState(true, state.StepSize, state.Threshold, state.DeltaMinutes, state.ColorScale, state.Current, state.History, state.Sensors);
    }

    [ReducerMethod]
    public static ARState ReduceFetchedFramesaction(ARState state, FetchedFramesAction action)
    {
        var current = action.FetchedFrames
            .DefaultIfEmpty(state.Current)
            .LastOrDefault();

        return new ARState(false, state.StepSize, state.Threshold, state.DeltaMinutes, state.ColorScale, current, action.FetchedFrames, action.FetchesSensors);
    }

    [ReducerMethod]
    public static ARState ReduceMoveBackwardAction(ARState state, MoveBackwardAction action)
    {
        var current = state.Current;
        var currentIndex = state.History.IndexOf(current);

        if (currentIndex - action.Count <= 0)
        {
            current = state.History.First();
        }
        else
        {
            current = state.History.ElementAt(currentIndex - action.Count);
        }

        return new ARState(false, state.StepSize, state.Threshold, state.DeltaMinutes, state.ColorScale, current, state.History, state.Sensors);
    }

    [ReducerMethod]
    public static ARState ReduceMoveForwardaction(ARState state, MoveForwardAction action)
    {
        var current = state.Current;
        var currentIndex = state.History.IndexOf(current);

        if (currentIndex + action.Count >= state.History.Count)
        {
            current = state.History.Last();
        }
        else
        {
            current = state.History.ElementAt(currentIndex + action.Count);
        }

        return new ARState(false, state.StepSize, state.Threshold, state.DeltaMinutes, state.ColorScale, current, state.History, state.Sensors);
    }

    [ReducerMethod]
    public static ARState ReduceApplySettingsAction(ARState state, ApplySettingsAction action)
    {
        return new ARState(false, action.StepSize, action.Threshold, action.DeltaMinutes, action.ColorScale, state.Current, state.History, state.Sensors);
    }
}
