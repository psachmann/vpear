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
    public ChangeSceneAction(int sceneId)
    {
        SceneId = sceneId;
    }

    public int SceneId { get; }
}
