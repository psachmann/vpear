using Autofac;
using Serilog;
using System;
using UnityEngine;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    private static readonly IContainer container = null;
    private ILifetimeScope scope = null;

    static AbstractBase()
    {
        ConfigureLogger();

        var builder = new ContainerBuilder();
        builder.RegisterModule(new ClientModule());
        builder.RegisterModule(new SerilogModule());

        container = builder.Build();
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();
    }

    protected virtual void Awake()
    {
        this.scope = container.BeginLifetimeScope();
    }

    public void Dispose()
    {
        this.scope?.Dispose();
        GC.SuppressFinalize(this);
    }
}
