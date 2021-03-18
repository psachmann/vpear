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
        var deltaMinutes = TimeSpan.FromMinutes(60f);
        var heatmap = new Heatmap(0, 0, TimeSpan.Zero, new List<GetFrameResponse>());

        return new ARState(false, current, history, deltaMinutes, heatmap);
    }
}
