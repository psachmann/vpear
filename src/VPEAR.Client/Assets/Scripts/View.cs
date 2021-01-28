using UnityEngine;

public class View : AbstractBase
{
    private Canvas canvas;
    private ViewService viewService;

    private void Awake()
    {
        this.canvas = this.GetComponent<Canvas>();
    }

    public void Init(ViewService viewService)
    {
        this.viewService = viewService;
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
