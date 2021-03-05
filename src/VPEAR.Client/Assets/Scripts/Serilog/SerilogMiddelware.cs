using Fluxor;
using Fluxor.Exceptions;
using Serilog;
using System.Threading.Tasks;

public class SerilogMiddelware : Middleware
{
    private readonly ILogger logger;
    private IStore store;

    public SerilogMiddelware(ILogger logger)
    {
        this.logger = logger;
    }

    public override Task InitializeAsync(IStore store)
    {
        this.store = store;
        this.store.UnhandledException += UnhandledExceptionHandler;
        this.logger.Information("Initialized store");

        return Task.CompletedTask;
    }

    public override bool MayDispatchAction(object action)
    {
        this.logger.Debug($"Dispatching: {action.GetType()}");

        return true;
    }

    private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
        Log.Error($"Message: {args.Exception.Message}\nStackTrace: {args.Exception.StackTrace}");
    }
}
