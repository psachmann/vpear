using Serilog;
using System;
using System.Collections.Generic;

public class NavigationService
{
    private static readonly Dictionary<string, AbstractView> s_views = new Dictionary<string, AbstractView>();
    private readonly Stack<AbstractView> _viewHistory;
    private readonly ILogger _logger;

    public NavigationService(ILogger logger)
    {
        _viewHistory = new Stack<AbstractView>();
        _logger = logger;
    }

    public AbstractView CurrentView 
    {
        get
        {
            return _viewHistory.Count > 0 ? _viewHistory.Peek() : null;
        }
    }

    public string CurrentViewName
    {
        get
        {
            return _viewHistory.Count > 0 ? _viewHistory.Peek().name : null;
        }
    }

    public static void RegisterView(AbstractView view)
    {
        s_views[view.name] = view;
    }

    public bool CanNavigateBack()
    {
        return _viewHistory.Count > 1;
    }

    public void NavigateBack()
    {
        if (CanNavigateBack())
        {
            CurrentView.Hide();
            _viewHistory.Pop();
            CurrentView.Show();
            _logger.Debug($"NavigateBack: {CurrentViewName}");
        }
        else
        {
            _logger.Debug("Can not navigate back.");
        }
    }

    public void NavigateTo(string viewName)
    {
        if (CurrentViewName == viewName)
        {
            return;
        }

        if (s_views.TryGetValue(viewName, out var view))
        {
            if (CurrentView != null)
            {
                CurrentView.Hide();
            }

            _viewHistory.Push(view);
            view.Show();
            _logger.Debug($"NavigateTo: {viewName}");
        }
        else
        {
            _logger.Debug($"NotFound: {viewName}");

            throw new ArgumentOutOfRangeException(nameof(viewName));
        }
    }
}
