using System;

public class ApplySettingsAction
{
    public ApplySettingsAction(int stepSize, float threshold, float deltaMinutes, ColorScale colorScale)
    {
        StepSize = stepSize;
        Threshold = threshold;
        DeltaMinutes = TimeSpan.FromMinutes(deltaMinutes);
        ColorScale = colorScale;
    }

    public int StepSize { get; }

    public float Threshold { get; }

    public TimeSpan DeltaMinutes { get; }

    public ColorScale ColorScale { get; }
}
