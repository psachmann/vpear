using UnityEngine.UI;

public class DeviceListScript : AbstractView
{
    private void Start()
    {
        var backButton = this.GetComponentInChildren<Button>();
        backButton.onClick.AddListener(this.GoBack);

        Logger.Debug($"Initialized {this.GetType()}");
    }

    private void GoBack()
    {
        var popup = this.viewService.GetViewByName(Constants.PopupViewName) as PopupScript;

        popup.Show("Popup Title!!", "Popup Message!!!", popup.Hide);

        this.viewService.GoBack();
    }
}
