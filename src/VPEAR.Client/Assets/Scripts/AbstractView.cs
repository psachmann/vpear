using System;
using UnityEngine;

public abstract class AbstractView : AbstractBase
{
    protected Canvas canvas;
    protected ViewService viewService;

    private void Awake()
    {
        this.canvas = this.GetComponent<Canvas>();
    }

    public void Init(ViewService viewService)
    {
        this.viewService = viewService;
        this.Hide();
    }

    public string GetName()
    {
        return this.canvas.name;
    }

    public virtual void Hide()
    {
        this.canvas.enabled = false;
    }

    public virtual void Show()
    {
        this.canvas.enabled = true;
    }

    public virtual void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
    }
}
