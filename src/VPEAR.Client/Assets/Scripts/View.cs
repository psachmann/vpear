using UnityEngine;

public class View : AbstractBase
{
    private Canvas view = null;
    private ViewManager viewManager;

    private void Awake()
    {
        this.view = this.GetComponent<Canvas>();
    }

    public void Initialize(ViewManager viewManager)
    {
        this.viewManager = viewManager;
        this.Hide();
    }

    public void Hide()
    {
        this.view.enabled = false;
    }

    public void Show()
    {
        this.view.enabled = true;
    }

    public string GetName()
    {
        return this.view.name;
    }
}
