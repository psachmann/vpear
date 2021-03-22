using UnityEngine;

public abstract class AbstractView : AbstractBase
{
    protected Canvas _canvas;

    protected override void Awake()
    {
        base.Awake();
        _canvas = GetComponent<Canvas>();
        Hide();
        NavigationService.RegisterView(this);
    }

    public virtual void Hide()
    {
        _canvas.enabled = false;
    }

    public virtual void Show()
    {
        _canvas.enabled = true;
    }
}
