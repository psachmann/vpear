using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.Http;
using UnityEngine;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using ILogger = Serilog.ILogger;

public abstract class AbstractBase : MonoBehaviour
{
    private static readonly object s_syncRoot = new object();
    private static bool s_isInitialized = false;
    protected static IServiceProvider s_provider;
    protected IDispatcher _dispatcher;
    protected ILogger _logger;

    protected virtual void Awake()
    {
        lock (s_syncRoot)
        {
            if (!s_isInitialized)
            {
                Initialize();
            }
        }

        _dispatcher = s_provider.GetRequiredService<IDispatcher>();
        _logger = s_provider.GetRequiredService<ILogger>();
    }

    private void Initialize()
    {
        // setting up logger
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Unity()
            .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

        // setting up http client
        var client = new VPEARClient(Constants.ServerBaseAddress, new HttpClient());

        // setting up dependency injection
        var services = new ServiceCollection()
            .AddLogging()
            .AddSingleton<ILogger>(logger)
            .AddSingleton<IVPEARClient>(client)
            .AddSingleton(new NavigationService(logger))
            .AddFluxor(builder => builder.ScanAssemblies(typeof(AbstractBase).Assembly)
            .AddMiddleware<SerilogMiddelware>());

        s_provider = services.BuildServiceProvider();
        s_provider.GetRequiredService<IStore>()
            .InitializeAsync()
            .GetAwaiter()
            .GetResult();
        s_isInitialized = true;
    }
}
