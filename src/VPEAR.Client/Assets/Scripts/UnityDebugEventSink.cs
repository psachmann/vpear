using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.IO;
using UnityEngine;

public class UnityDebugEventSink : ILogEventSink
{
    private readonly ITextFormatter formatter = new MessageTemplateTextFormatter("{Message}");

    public void Emit(LogEvent logEvent)
    {
        using (var buffer = new StringWriter())
        {
            this.formatter.Format(logEvent, buffer);

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                    Debug.Log(buffer.ToString().Trim());
                    break;
                case LogEventLevel.Warning:
                    Debug.LogWarning(buffer.ToString().Trim());
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    Debug.LogError(buffer.ToString().Trim());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logEvent.Level), "Unknown log level");
            }
        }
    }
}
