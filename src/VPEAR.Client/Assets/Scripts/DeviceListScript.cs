using System.Linq;
using UnityEngine.UI;

public class DeviceListScript : AbstractView
{
    private Button itemTemplate = null;

    private void Start()
    {
        var buttons = this.GetComponentsInChildren<Button>();
        this.itemTemplate = buttons.First(button => string.Equals(button.name, Constants.DeviceListItemName));
        this.itemTemplate.onClick.AddListener(() => this.OnItemClick());
    }

    private void OnItemClick()
    {
        this.viewService.GoTo(Constants.DeviceDetailViewName);
    }
}
