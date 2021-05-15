using System;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class ARState
{
    public ARState(
        bool isLoading,
        int stepSize,
        float threshold,
        TimeSpan deltaMinutes,
        ColorScale colorScale,
        GetFrameResponse current,
        IList<GetFrameResponse> history,
        IList<GetSensorResponse> sensors)
    {
        IsLoading = isLoading;
        StepSize = stepSize;
        Threshold = threshold;
        DeltaMinutes = deltaMinutes;
        ColorScale = colorScale;
        Current = current;
        History = history;
        Sensors = sensors;
    }

    public bool IsLoading { get; }

    public int StepSize { get; }

    public float Threshold { get; }

    public TimeSpan DeltaMinutes { get; }

    public ColorScale ColorScale { get; }

    public GetFrameResponse Current { get; }

    public IList<GetFrameResponse> History { get; }

    public IList<GetSensorResponse> Sensors { get; }
}
