using Serilog;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NavigationService
{
    private static readonly Dictionary<string, AbstractView> s_views = new Dictionary<string, AbstractView>();
    private readonly Stack<AbstractView> _history;
    private readonly ILogger _logger;

    public NavigationService(ILogger logger)
    {
        _history = new Stack<AbstractView>();
        _logger = logger;
    }

    public AbstractView Location 
    {
        get
        {
            return _history.Count > 0 ? _history.Peek() : null;
        }
    }

    public string LocationName
    {
        get
        {
            return _history.Count > 0 ? _history.Peek().name : null;
        }
    }

    public static void RegisterView(AbstractView view)
    {
        s_views[view.name] = view;
    }

    public bool CanNavigateBack()
    {
        // it must be 2, because if its one, we can navigate back to the login view
        // and we don't want this, if we logout the history will be cleared and we won't
        // be able the access other user data
        return _history.Count > 2;
    }

    public void NavigateBack()
    {
        if (CanNavigateBack())
        {
            Location.Hide();
            _history.Pop();
            Location.Show();
            _logger.Information($"NavigateBack: {LocationName}");
        }
        else
        {
            _logger.Debug("Can not navigate back.");
        }
    }

    public void NavigateTo(string viewName)
    {
        if (LocationName == viewName)
        {
            return;
        }

        if (s_views.TryGetValue(viewName, out var view))
        {
            if (Location != null)
            {
                Location.Hide();
            }

            _history.Push(view);
            view.Show();
            _logger.Information($"NavigateTo: {viewName}");
        }
        else
        {
            _logger.Error($"ViewNotFound: {viewName}");

            throw new ArgumentOutOfRangeException(nameof(viewName));
        }
    }

    public void ClearHistory()
    {
        _history.Clear();
    }

    public void ChangeScene(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid())
        {
            _logger.Information($"GoToScene: {scene.name}");

            SceneManager.LoadScene(scene.name);
        }
        else
        {
            _logger.Error($"SceneNotFound: {scene.name}");

            throw new ArgumentOutOfRangeException(nameof(sceneName));
        }
    }
}
