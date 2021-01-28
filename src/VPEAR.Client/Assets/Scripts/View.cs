using UnityEngine;

public abstract class View : AbstractBase
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

    public void Hide()
    {
        this.canvas.enabled = false;
    }

    public void Show()
    {
        this.canvas.enabled = true;
    }
}
