using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.Http;
using UnityEngine;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    private readonly static IServiceProvider Provider;
    protected static Serilog.ILogger Logger;
    protected static VPEARClient Client;

    private IServiceScope scope;
    private IStore store;

    static AbstractBase()
    {
        // setting up logger
        Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Unity()
            .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

        // setting up http client
        Client = new VPEARClient(Constants.ServerBaseAddress, new HttpClient());

        // setting up dependency injection
        Provider = new ServiceCollection()
            .AddSingleton(Logger)
            .AddSingleton(Client)
            .AddFluxor(builder => builder.ScanAssemblies(typeof(AbstractBase).Assembly))
            .BuildServiceProvider();

    }

    protected IServiceScope Scope
    {
        get
        {
            return this.scope;
        }
    }

    protected IStore Store
    {
        get
        {
            return this.store;
        }
    }

    protected virtual void Awake()
    {
        this.scope = Provider.CreateScope();
        this.store = this.scope.ServiceProvider.GetRequiredService<IStore>();
    }

    public void Dispose()
    {
        this.scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
