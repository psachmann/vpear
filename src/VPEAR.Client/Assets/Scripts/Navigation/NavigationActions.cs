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

public class ChangeSceneAction
{
    public ChangeSceneAction(string sceneName)
    {
        SceneName = sceneName;
    }

    public string SceneName { get; }
}
