using System.Collections.Generic;
using System.Linq;

public class ViewService : AbstractBase
{
    private readonly Stack<AbstractView> viewHistory = new Stack<AbstractView>();

    private void Start()
    {
        this.viewHistory.Push(null); // prevents stack empty exception
        this.Init();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void Init()
    {
        var views = this.GetComponentsInChildren<AbstractView>();

        foreach (var view in views)
        {
            view.Init(this);
        }

        var startView = views.Where(view => string.Equals(view.GetName(), Constants.LoginViewName))
            .First();

        this.GoTo(startView);
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
        Logger.Debug("Go back");

        if (this.CanGoBack())
        {
            this.viewHistory.Pop().Hide();
            this.viewHistory.Peek().Show();
        }
    }

    public void GoTo(AbstractView nextView)
    {
        Logger.Debug($"Go to {nextView.GetName()}");

        this.viewHistory.Peek()?.Hide();
        this.viewHistory.Push(nextView);
        this.viewHistory.Peek().Show();
    }
}
