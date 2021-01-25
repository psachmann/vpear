using System.Collections.Generic;
using System.Linq;

public class ViewManager : AbstractBase
{
    private readonly Stack<View> history = new Stack<View>();

    public View Current = null;

    private void Start()
    {
        this.Initialize();
    }

    private void Initialize()
    {
        var views = this.GetComponentsInChildren<View>();

        foreach (var view in views)
        {
            view.Initialize(this);
        }

        var startView = views.Where(view => string.Equals(view.GetName(), Constants.LoginViewName))
            .First();

        this.GoTo(startView);
    }

    public void GoBack()
    {
        this.history.Pop().Hide();
        this.Current = this.history.Peek();
        this.Current.Show();
    }

    public void GoTo(View nextView)
    {
        this.Current.Hide();
        this.history.Push(nextView);
        this.Current = nextView;
        this.Current.Show();
    }
}
