using System;

public class NavigateEventArgs<TPayload> : EventArgs
{
    public NavigateEventArgs(AbstractView from, AbstractView to)
    {
        this.From = from;
        this.To = to;
        this.Payload = default;
    }

    public NavigateEventArgs(AbstractView from, AbstractView to, TPayload payload)
    {
        this.From = from;
        this.To = to;
        this.Payload = payload;
    }

    public AbstractView From { get; }

    public AbstractView To { get; }

    public TPayload Payload { get; }
}
