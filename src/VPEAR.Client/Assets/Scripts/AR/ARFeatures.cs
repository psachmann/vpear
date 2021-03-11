using Fluxor;

public class ARFeature : Feature<ARState>
{
    public override string GetName()
    {
        return nameof(ARState);
    }

    protected override ARState GetInitialState()
    {
        return new ARState();
    }
}
