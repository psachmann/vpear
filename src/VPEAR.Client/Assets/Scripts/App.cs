using Fluxor;
using Serilog;
using System;
using System.Net.Http;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

public class App : IDisposable
{
    static App()
    {
        ConfigureLogger();
        ConfigureClient();
    }

    public App(
        IStore store,
        IDispatcher dispatcher,
        IState<LoginState> loginState)
    {
        this.Store = store;
        this.Dispatcher = dispatcher;
        this.LoginState = loginState;
    }

    public static IVPEARClient Client { get; private set; }

    public static ILogger Logger { get; private set; }

    public IStore Store { get; }

    public IDispatcher Dispatcher { get; }

    public IState<LoginState> LoginState { get; }

    public void Dispose()
    {
        Client?.Dispose();
        Log.CloseAndFlush();
        GC.SuppressFinalize(this);

        Logger = null;
        Client = null;
    }

    public void Run()
    {
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
}
