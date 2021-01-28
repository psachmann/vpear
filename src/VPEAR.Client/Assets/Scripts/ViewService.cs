using System.Collections.Generic;
using System.Linq;

public class ViewService : AbstractBase
{
    private readonly Stack<View> viewHistory = new Stack<View>();

    private void Start()
    {
        this.viewHistory.Push(null); // prevents stack empty exception
        this.Init();
    }

    private void Init()
    {
        var views = this.GetComponentsInChildren<View>();

        foreach (var view in views)
        {
            view.Init(this);
        }

        var startView = views.Where(view => string.Equals(view.GetName(), Constants.LoginViewName))
            .First();

        this.GoTo(startView);
    }

    public View GetCurrentView()
    {
        return this.viewHistory.Peek();
    }

    public bool CanGoBack()
    {
        return this.viewHistory.Count > 0;
    }

    public void GoBack()
    {
        if (this.CanGoBack())
        {
            this.viewHistory.Pop().Hide();
            this.viewHistory.Peek().Show();
        }
    }

    public void GoTo(View nextView)
    {
        this.viewHistory.Peek()?.Hide();
        this.viewHistory.Push(nextView);
        this.viewHistory.Peek().Show();
    }
}
