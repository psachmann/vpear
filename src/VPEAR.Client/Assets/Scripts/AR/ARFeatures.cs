using Fluxor;
using System;
using System.Collections.Generic;
using UnityEngine;
using VPEAR.Core.Wrappers;

public class ARFeature : Feature<ARState>
{
    public override string GetName()
    {
        return nameof(ARState);
    }

    protected override ARState GetInitialState()
    {
        var current = new GetFrameResponse();
        var history = new List<GetFrameResponse>();
        var sensors = new List<GetSensorResponse>();
        var deltaMinutes = TimeSpan.FromMinutes(60f);
        var stepSize = 10;
        var treshold = 80f;

        return new ARState(false, stepSize, treshold, deltaMinutes, ColorScale.RedToGreen, current, history, sensors);
    }
}
