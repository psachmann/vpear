using Serilog;
using System;
using System.Net.Http;
using UnityEngine;
using VPEAR.Core;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    protected static Serilog.ILogger Logger = null;
    protected static VPEARClient Client = null;

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
            .WriteTo.Unity()
            .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

        Logger = Log.Logger;
    }

    private static void ConfigureClient()
    {
        Client = new VPEARClient(Constants.ServerBaseAddress, new HttpClient());
    }


    public void Dispose()
    {
        Client?.Dispose();
        Log.CloseAndFlush();
        GC.SuppressFinalize(this);

        Logger = null;
        Client = null;
    }
}
