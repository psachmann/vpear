using UnityEngine;

public partial class ViewService : AbstractBase
{
    private void Start()
    {
        this.Init();

        Logger.Debug($"Initialized {this.GetType()}");
    }
/*
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
*/
}
