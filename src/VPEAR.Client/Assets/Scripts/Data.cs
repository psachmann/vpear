using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

public static class Data
{
    public static IList<GetFrameResponse> CreateHistory(int width, int height)
    {
        var seed = DateTimeOffset.Now;
        var frequency = TimeSpan.FromSeconds(10f);
        var count = 1000;
        var timestamps = CreateTimestamps(seed, frequency, count);

        return timestamps.Select(timestamp => new GetFrameResponse()
        {
            Filter = new GetFiltersResponse()
            {
                Noise = true,
                Smooth = true,
                Spot = true,
            },
            Readings = CreateReadings(width, height),
            Time = timestamp,
        }).ToList();
    }

    public static IList<GetSensorResponse> CreateSensors()
    {
        var sensors = new List<GetSensorResponse>();

        sensors.Add(new GetSensorResponse()
        {
            Columns = 27,
            Height = 180,
            Maximum = 100,
            Minimum = 0,
            Name = "BodiTrak 2 Pro",
            Rows = 64,
            Units = "mmhg",
            Width = 80,
        });

        return sensors;
    }

    private static IEnumerable<DateTimeOffset> CreateTimestamps(DateTimeOffset seed, TimeSpan delta, int count)
    {
        var result = new List<DateTimeOffset>();

        for (var i = 0; i < count; i++)
        {
            result.Add(seed);
            seed += delta;
        }

        return result;
    }

    public static IList<IList<int>> CreateReadings(int width, int height)
    {
        var result = new List<IList<int>>(width);
        var counter = 0;

        foreach (var x in Enumerable.Range(0, width))
        {
            if (x < (width / 2))
            {
                counter += 3;
            }
            else
            {
                counter -= 3;
            }

            result.Add(Enumerable.Repeat(counter, height).ToList());
        }

        return result;
    }
}
