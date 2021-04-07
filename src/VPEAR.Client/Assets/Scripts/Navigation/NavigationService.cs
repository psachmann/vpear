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
        return _history.Count > 1;
    }

    public void NavigateBack()
    {
        if (CanNavigateBack())
        {
            Location.Hide();
            _history.Pop();
            Location.Show();
            _logger.Debug($"NavigateBack: {LocationName}");
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
