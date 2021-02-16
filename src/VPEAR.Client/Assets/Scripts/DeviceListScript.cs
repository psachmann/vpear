using UnityEngine;
using UnityEngine.UI;

public class DeviceListScript : AbstractView
{
    [SerializeField] private Button itemTemplate = null;

    private void Start()
    {
        this.itemTemplate.onClick.AddListener(() => this.OnItemClick());
    }

    private void OnItemClick()
    {
        this.viewService.GoTo(Constants.DeviceDetailViewName);
    }
}
