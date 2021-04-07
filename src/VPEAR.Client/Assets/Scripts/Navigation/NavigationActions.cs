public class NavigateToAction
{
    public NavigateToAction(string nextView, bool executeNavigation = true)
    {
        ExecuteNavigation = executeNavigation;
        NextView = nextView;
    }

    public bool ExecuteNavigation { get; }

    public string NextView { get; }
}

public class NavigateBackAction
{
}

public class ChangeSceneAction
{
    public ChangeSceneAction(int sceneId)
    {
        SceneId = sceneId;
    }

    public int SceneId { get; }
}
