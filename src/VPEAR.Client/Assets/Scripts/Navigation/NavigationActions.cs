public class NavigateToAction
{
    public NavigateToAction(AbstractView from, AbstractView to)
    {
        this.From = from;
        this.To = to;
    }

    public AbstractView From { get; }

    public AbstractView To { get; }
}

public class NavigateBackAction
{
}
