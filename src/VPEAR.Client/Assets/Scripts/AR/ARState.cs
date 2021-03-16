using System;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class ARState
{
    public ARState(
        bool isLoading,
        GetFrameResponse current,
        IEnumerable<GetFrameResponse> history,
        TimeSpan deltaMinutes,
        GridMesh gridMesh,
        ColorScale colorScale = default)
    {
        IsLoading = isLoading;
        Current = current;
        History = history;
        DeltaMinutes = deltaMinutes;
        GridMesh = gridMesh;
        ColorScale = colorScale;
    }

    public bool IsLoading { get; }

    public GetFrameResponse Current { get; }

    public IEnumerable<GetFrameResponse> History { get; }

    public TimeSpan DeltaMinutes { get; }

    public GridMesh GridMesh { get; }

    public ColorScale ColorScale { get; }
}
