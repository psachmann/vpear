using System;

public class NavigateEventArgs : EventArgs
{
    public NavigateEventArgs(AbstractView from, AbstractView to)
    {
        this.From = from;
        this.To = to;
    }

    public AbstractView From { get; }

    public AbstractView To { get; }
}
