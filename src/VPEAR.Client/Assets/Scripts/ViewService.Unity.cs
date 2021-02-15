using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class ViewService : AbstractBase
{
    private CanvasGroup navigationPanel = null;
    private Button devices = null;
    private Button users = null;
    private Button settings = null;

    private void Start()
    {
        var panels = this.GetComponentsInChildren<CanvasGroup>();
        this.navigationPanel = panels.First(panel => string.Equals(panel.name, Constants.NavigationPanelName));
        this.HideContent();

        var buttons = this.GetComponentsInChildren<Button>();
        this.devices = buttons.First(button => string.Equals(button.name, Constants.DevicesButtonName));
        this.devices.onClick.AddListener(() => this.GoTo(Constants.DeviceListViewName));
        this.users = buttons.First(button => string.Equals(button.name, Constants.UsersButtonName));
        this.users.onClick.AddListener(() => this.GoTo(Constants.UserListViewName));
        this.settings = buttons.First(button => string.Equals(button.name, Constants.SettingsButtonName));
        this.settings.onClick.AddListener(() => this.GoTo(Constants.SettingsViewName));

        this.Init();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void Update()
    {
        if ( /*Application.platform == RuntimePlatform.Android*/ true
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
}
