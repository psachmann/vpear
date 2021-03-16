using Fluxor;
using System.Linq;

public static partial class Reducers
{
    [ReducerMethod]
    public static ARState ReduceFetchingFramesAction(ARState state, FetchingFramesAction action)
    {
        return new ARState(true, state.Current, state.History, state.DeltaMinutes, state.GridMesh, state.ColorScale);
    }

    [ReducerMethod]
    public static ARState ReduceFetchedFramesaction(ARState state, FetchedFramesAction action)
    {
        var current = action.FetchedFrames
            .DefaultIfEmpty(state.Current)
            .LastOrDefault();

        return new ARState(false, current, action.FetchedFrames, state.DeltaMinutes, state.GridMesh, state.ColorScale);
    }
}
