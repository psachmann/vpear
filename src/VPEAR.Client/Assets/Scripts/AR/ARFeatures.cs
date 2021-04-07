using Fluxor;
using System;
using System.Linq;

public class ARFeature : Feature<ARState>
{
    public override string GetName()
    {
        return nameof(ARState);
    }

    protected override ARState GetInitialState()
    {
        var history = Data.CreateHistory(64, 27);
        var sensors = Data.CreateSensors();
        var deltaMinutes = TimeSpan.FromMinutes(60f);
        var stepSize = 10;
        var treshold = 80f;

        return new ARState(false, stepSize, treshold, deltaMinutes, ColorScale.Plasma, history.Last(), history, sensors);
    }
}
