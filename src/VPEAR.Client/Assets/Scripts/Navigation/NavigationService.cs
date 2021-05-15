using Serilog;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NavigationService
{
    private static readonly Dictionary<string, AbstractView> s_views = new Dictionary<string, AbstractView>();
    private readonly Stack<string> _history = new Stack<string>();
    private readonly ILogger _logger;

    public NavigationService(ILogger logger)
    {
        _logger = logger;
    }

    public string Location
    {
        get
        {
            return _history.Count > 0 ? _history.Peek() : null;
        }
    }

    public static void RegisterView(AbstractView view)
    {
        s_views[view.name] = view;
    }

    public bool CanNavigateBack()
    {
        return _history.Count > 2;
    }

    public void NavigateBack()
    {
        if (CanNavigateBack())
        {
            s_views[Location].Hide();
            _history.Pop();
            s_views[Location].Show();
            _logger.Debug($"NavigateBack: {Location}");
        }
        else
        {
            _logger.Debug("Can not navigate back.");
        }
    }

    public void NavigateTo(string viewName)
    {
        if (s_views.TryGetValue(viewName, out var view))
        {
            if (Location != null)
            {
                s_views[Location].Hide();
            }

            _history.Push(view.name);
            s_views[Location].Show();
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

        foreach (var view in s_views)
        {
            view.Value.Hide();
        }
    }

    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
