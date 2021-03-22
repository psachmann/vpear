public class ApplySettingsAction
{
    public ApplySettingsAction(float deltaMinutes, ColorScale colorScale)
    {
        DeltaMinutes = deltaMinutes;
        ColorScale = colorScale;
    }

    public float DeltaMinutes { get; }

    public ColorScale ColorScale { get; }
}
