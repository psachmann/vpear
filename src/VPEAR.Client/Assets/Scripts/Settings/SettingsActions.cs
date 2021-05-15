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

public class ChangePasswordAction
{
    public ChangePasswordAction(string userName, string oldPassword, string newPassword)
    {
        UserName = userName;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    public string UserName { get; }

    public string OldPassword { get; }

    public string NewPassword { get; }
}
