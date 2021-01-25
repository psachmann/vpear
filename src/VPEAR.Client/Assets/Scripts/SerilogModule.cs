using Autofac;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

internal class SerilogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(_ => new LoggerFactory(
            new ILoggerProvider[]
            {
                new SerilogLoggerProvider()
            }))
            .As<ILoggerProvider>()
            .SingleInstance();

        builder.RegisterGeneric(typeof(Logger<>))
            .As(typeof(ILogger<>))
            .SingleInstance();
    }
}
