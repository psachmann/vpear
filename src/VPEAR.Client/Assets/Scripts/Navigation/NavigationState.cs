using System.Collections.Generic;

public class NavigationState
{
    public NavigationState(string viewName)
    {
        this.ViewName = viewName;
    }

    public string ViewName { get; }
}
