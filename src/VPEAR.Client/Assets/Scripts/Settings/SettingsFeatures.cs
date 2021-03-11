using Fluxor;

public class SettingsFeature : Feature<SettingsState>
{
    public override string GetName()
    {
        return nameof(SettingsState);
    }

    protected override SettingsState GetInitialState()
    {
        return new SettingsState();
    }
}
