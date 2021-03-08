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
    private static readonly IStore s_store;
    protected static readonly IServiceProvider s_provider;
    protected IDispatcher _dispatcher;
    protected ILogger _logger;

    static AbstractBase()
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
        s_store = s_provider.GetRequiredService<IStore>();
        s_store.InitializeAsync().Wait();
    }

    protected virtual void Awake()
    {
        _dispatcher = s_provider.GetRequiredService<IDispatcher>();
        _logger = s_provider.GetRequiredService<ILogger>();
    }
}
