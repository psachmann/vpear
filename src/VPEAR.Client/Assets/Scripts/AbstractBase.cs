using Serilog;
using System;
using System.Net.Http;
using UnityEngine;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    protected static Serilog.ILogger logger = null;
    protected static IVPEARClient client = null;

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

        logger = Log.Logger;
    }

    private static void ConfigureClient()
    {
        client = new VPEARClient(Constants.ServerBaseAddress, new HttpClient());
    }


    public void Dispose()
    {
        client?.Dispose();
        Log.CloseAndFlush();
        GC.SuppressFinalize(this);

        logger = null;
        client = null;
    }
}
