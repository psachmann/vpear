public class NavigateToAction
{
    public NavigateToAction(string nextView)
    {
        this.NextView = nextView;
    }

    public string NextView { get; }
}

public class NavigateBackAction
{
}
