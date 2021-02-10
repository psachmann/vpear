using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewService : AbstractBase
{
    private readonly Stack<AbstractView> viewHistory = new Stack<AbstractView>();

    private void Start()
    {
        this.viewHistory.Push(null); // prevents stack empty exception
        this.Init();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android
            && Input.GetKeyDown(KeyCode.Escape)
            && this.CanGoBack())
        {
            this.GoBack();
        }
        else
        {
            Application.Quit();
        }
    }

    public void Init()
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        foreach (var view in views)
        {
            view.Init(this);
        }

        this.GoTo(Constants.UserListViewName);
    }

    public AbstractView GetCurrentView()
    {
        return this.viewHistory.Peek();
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
        Logger.Debug("Try go back");

        if (this.CanGoBack())
        {
            Logger.Debug("Go back");
            this.viewHistory.Pop().Hide();
            this.viewHistory.Peek().Show();
        }
    }

    public void GoTo(string nextViewName)
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        this.GoTo(views.First(view => string.Equals(view.GetName(), nextViewName)));
    }

    public void GoTo(AbstractView nextView)
    {
        Logger.Debug($"Go to {nextView.GetName()}");

        this.viewHistory.Peek()?.Hide();
        this.viewHistory.Push(nextView);
        this.viewHistory.Peek().Show();
    }
}
