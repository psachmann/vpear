using UnityEngine.UI;

public class DeviceListViewScript : View
{
    private void Start()
    {
        var backButton = this.GetComponentInChildren<Button>();

        backButton.onClick.AddListener(this.GoBack);
    }

    private void GoBack()
    {
        this.viewService.GoBack();
    }
}
