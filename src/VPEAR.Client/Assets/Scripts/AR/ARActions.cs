using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class FetchingFramesAction
{
    public FetchingFramesAction(GetDeviceResponse device)
    {
        Device = device;
    }

    public GetDeviceResponse Device { get; }
}

public class FetchedFramesAction
{
    public FetchedFramesAction(IList<GetFrameResponse> fetchedFrames, IList<GetSensorResponse> fetchedSensors)
    {
        FetchedFrames = fetchedFrames;
        FetchesSensors = fetchedSensors;
    }

    public IList<GetFrameResponse> FetchedFrames { get; }

    public IList<GetSensorResponse> FetchesSensors { get; }
}

public class MoveBackwardAction
{
    public MoveBackwardAction(int count)
    {
        Count = count;
    }

    public int Count { get; }
}

public class MoveForwardAction
{
    public MoveForwardAction(int count)
    {
        Count = count;
    }

    public int Count { get; }
}
