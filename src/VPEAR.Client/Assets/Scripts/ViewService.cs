using System;
using System.Collections.Generic;
using System.Linq;
using VPEAR.Core;

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

        // this.OnNavigate(new NavigateEventArgs<Null>(from, to));

        from.Hide();
        to.Show();

        Logger.Debug("Go back");
    }

    public void GoTo(string viewName)
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        this.GoTo<Null>(views.First(view => string.Equals(view.GetName(), viewName)), default);
    }

    public void GoTo<TPayload>(string viewName, TPayload payload)
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        this.GoTo(views.First(view => string.Equals(view.GetName(), viewName)), payload);
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

    private void GoTo<TPayload>(AbstractView view, TPayload payload)
    {
        Logger.Debug($"Go to {view.GetName()}");

        if (this.viewHistory.Count > 0)
        {
            this.OnNavigate(new NavigateEventArgs<TPayload>(this.viewHistory.Peek(), view, payload));
            this.viewHistory.Peek().Hide();
            this.viewHistory.Push(view);
            this.viewHistory.Peek().Show();
        }
        else
        {
            this.OnNavigate(new NavigateEventArgs<TPayload>(null, view, payload));
            this.viewHistory.Push(view);
            this.viewHistory.Peek().Show();
        }
    }

    private void OnNavigate(EventArgs eventArgs)
    {
        this.NavigateEvent?.Invoke(this, eventArgs);
    }
}
