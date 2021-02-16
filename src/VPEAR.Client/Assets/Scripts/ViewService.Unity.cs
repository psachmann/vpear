using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class ViewService : AbstractBase
{
    [SerializeField] private CanvasGroup navigationPanel = null;
    [SerializeField] private Button devices = null;
    [SerializeField] private Button users = null;
    [SerializeField] private Button settings = null;

    private void Start()
    {
        this.HideContent();
        this.devices.onClick.AddListener(() => this.GoTo(Constants.DeviceListViewName));
        this.users.onClick.AddListener(() => this.GoTo(Constants.UserListViewName));
        this.settings.onClick.AddListener(() => this.GoTo(Constants.SettingsViewName));
        this.Init();

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.CanGoBack())
        {
            this.GoBack();
        }
        else
        {
            Application.Quit();
        }
    }
}
