using System.Collections.Generic;

public class NavigationState
{
    public NavigationState(AbstractView current, Stack<AbstractView> history)
    {
        this.Current = current;
        this.History = history;
    }

    public AbstractView Current { get; }

    public Stack<AbstractView> History { get; }
}
