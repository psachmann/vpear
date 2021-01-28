using UnityEngine.UI;

public class LoginViewScript : View
{
    private void Start()
    {
        var loginButton = this.GetComponentInChildren<Button>();

        loginButton.onClick.AddListener(LoginUser);
    }

    private void LoginUser()
    {
        var view = this.viewService.GetViewByName(Constants.DeviceListViewName);

        logger.Information("Go to 'DeviceListView'");

        this.viewService.GoTo(view);
    }
}
