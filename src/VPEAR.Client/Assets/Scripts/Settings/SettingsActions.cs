using System;

public class ApplySettingsAction
{
    public ApplySettingsAction(int stepSize, float deltaMinutes, ColorScale colorScale)
    {
        StepSize = stepSize;
        DeltaMinutes = TimeSpan.FromMinutes(deltaMinutes);
        ColorScale = colorScale;
    }

    public int StepSize { get; }

    public TimeSpan DeltaMinutes { get; }

    public ColorScale ColorScale { get; }
}
