using Fluxor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

public class FetchingFramesEffect : Effect<FetchingFramesAction>
{
    private readonly IVPEARClient _client;
    private readonly ILogger _logger;

    public FetchingFramesEffect(IVPEARClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchingFramesAction action, IDispatcher dispatcher)
    {
        var frames = await _client.GetFramesAsync(action.Device.Id, 0, 1);
        var sensor = await _client.GetSensorsAsync(action.Device.Id);

        if (frames != null)
        {
            // fetching all frames from the server with FetchAllFramesAsync
            // fetching multiple times, because server sends max 100 frames at once
            var result = new List<GetFrameResponse>();

            for (var i = 0; i <= frames.Count; i += Constants.MaxSendFrames)
            {
                var container = await _client.GetFramesAsync(action.Device.Id, i, Constants.MaxSendFrames);

                result.AddRange(container.Items);
            }

            dispatcher.Dispatch(new FetchedFramesAction(result.OrderBy(frame => frame.Time).ToList(), IfNullReturnDefault(sensor?.Items)));
        }
        else
        {
            _logger.Error(_client.ErrorMessage);
            _logger.Information(_client?.Error.StackTrace);

            dispatcher.Dispatch(new FetchedFramesAction(new List<GetFrameResponse>(), new List<GetSensorResponse>()));
            dispatcher.Dispatch(new ShowPopupAction(Constants.ConnectionErrorTitleText, _client.ErrorMessage,
                () => dispatcher.Dispatch(new ClosePopupAction())));
        }
    }

    private static IList<GetSensorResponse> IfNullReturnDefault(IList<GetSensorResponse> sensors)
    {
        if (sensors != null)
        {
            return sensors;
        }
        else
        {
            return new List<GetSensorResponse>()
            {
                new GetSensorResponse()
                {
                    Columns = 16,
                    Height = 430,
                    Maximum = 100,
                    Minimum = 0,
                    Name = "default",
                    Rows = 16,
                    Units = "mmhg",
                    Width = 430,
                }
            };
        }
    }
}
