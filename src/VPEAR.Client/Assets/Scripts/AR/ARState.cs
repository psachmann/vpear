using System;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class ARState
{
    public ARState(
        bool isLoading,
        GetFrameResponse current,
        IEnumerable<GetFrameResponse> history,
        TimeSpan delta,
        Heatmap heatmap,
        ColorScale colorScale = default)
    {
        IsLoading = isLoading;
        Current = current;
        History = history;
        Delta = delta;
        Heatmap = heatmap;
        ColorScale = colorScale;
    }

    public bool IsLoading { get; }

    public GetFrameResponse Current { get; }

    public IEnumerable<GetFrameResponse> History { get; }

    public TimeSpan Delta { get; }

    public Heatmap Heatmap { get; }

    public ColorScale ColorScale { get; }
}
