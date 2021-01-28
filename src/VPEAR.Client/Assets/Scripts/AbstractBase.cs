using Serilog;
using Serilog.Events;
using System;
using UnityEngine;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    static AbstractBase()
    {
        ConfigureLogger();
        ConfigureClient();
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();
    }

    private static void ConfigureClient()
    {
        // throw new NotImplementedException();
    }


    public void Dispose()
    {
        Log.CloseAndFlush();
        GC.SuppressFinalize(this);
    }
}
