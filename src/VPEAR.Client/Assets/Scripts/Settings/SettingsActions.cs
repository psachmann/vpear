public class ApplySettingsAction
{
    public ApplySettingsAction(int stepSize, float deltaMinutes, ColorScale colorScale)
    {
        StepSize = stepSize;
        DeltaMinutes = deltaMinutes;
        ColorScale = colorScale;
    }

    public int StepSize { get; }

    public float DeltaMinutes { get; }

    public ColorScale ColorScale { get; }
}
