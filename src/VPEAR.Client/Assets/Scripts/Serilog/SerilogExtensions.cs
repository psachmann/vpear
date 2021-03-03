using Serilog;
using Serilog.Configuration;

public static class SerilogExtensions
{
    public static LoggerConfiguration Unity(this LoggerSinkConfiguration configuration)
    {
        return configuration.Sink<UnityDebugEventSink>();
    }
}
