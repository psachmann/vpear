using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private readonly Stack<View> history = new Stack<View>();

    public View Current = null;

    // Start is called before the first frame update
    private void Start()
    {
        this.Current = this.GetComponent<View>();
        this.Initialize();
    }

    private void Initialize()
    {
        var views = this.GetComponentsInChildren<View>();

        foreach (var view in views)
        {
            view.Initialize(this);
        }

        this.history.Push(this.Current);
        this.Current.Show();
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
