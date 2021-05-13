using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VPEAR.Core.Extensions;
using VPEAR.Core.Wrappers;

public static class Data
{
    public static IList<GetFrameResponse> CreateHistory()
    {
        GetReadings();

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
            Readings = GetReadings(),
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
            Name = "BodiTrak2 Pro",
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

    private static string ReadingsCache = null;

    public static IList<IList<int>> GetReadings()
    {
        if (ReadingsCache == null)
        {
            var path = Path.GetFullPath("./Assets/Json/sample_1.json");

            ReadingsCache = File.ReadAllText(path);
        }

        var values = ReadingsCache.FromJsonString<IList<int>>();
        var readings = new List<IList<int>>();

        for (var i = 0; i < values.Count; i += 27)
        {
            var temp = new List<int>(values
                .Skip(i)
                .Take(27));

            readings.Add(temp);
        }

        return readings;
    }
}
