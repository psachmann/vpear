using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class ViewService : AbstractBase
{
    private readonly Stack<AbstractView> viewHistory = new Stack<AbstractView>();
    
    public event EventHandler NavigateEvent;

    public void Init()
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        foreach (var view in views)
        {
            view.Init(this);
            this.NavigateEvent += view.NavigateEventHandler;
        }

        this.GoTo(Constants.LoginViewName);
    }

    public AbstractView GetViewByName(string viewName)
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        return views.FirstOrDefault(view => string.Equals(view.GetName(), viewName));
    }

    public bool CanGoBack()
    {
        return this.viewHistory.Count > 0;
    }

    public void GoBack()
    {
        var from = this.viewHistory.Pop();
        var to = this.viewHistory.Peek();

        this.OnNavigate(new NavigateEventArgs(from, to));

        from.Hide();
        to.Show();

        Logger.Debug("Go back");
    }

    public void GoTo(string viewName)
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        this.GoTo(views.First(view => string.Equals(view.GetName(), viewName)));
    }

    public void HideContent()
    {
        this.navigationPanel.alpha = 0;
        this.navigationPanel.interactable = false;
    }

    public void ShowContent()
    {
        this.navigationPanel.interactable = true;
        this.navigationPanel.alpha = 1;
    }

    private void GoTo(AbstractView view)
    {
        Logger.Debug($"Go to {view.GetName()}");

        if (this.viewHistory.Count > 0)
        {
            this.OnNavigate(new NavigateEventArgs(this.viewHistory.Peek(), view));
            this.viewHistory.Peek().Hide();
            this.viewHistory.Push(view);
            this.viewHistory.Peek().Show();
        }
        else
        {
            this.OnNavigate(new NavigateEventArgs(null, view));
            this.viewHistory.Push(view);
            this.viewHistory.Peek().Show();
        }
    }

    private void OnNavigate(EventArgs eventArgs)
    {
        this.NavigateEvent?.Invoke(this, eventArgs);
    }
}
