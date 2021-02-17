using UnityEngine;
using UnityEngine.UI;

public class DeviceListScript : AbstractView
{
    [SerializeField] private Button addButton = null;
    [SerializeField] private Button itemTemplate = null;

    private void Start()
    {
        this.addButton.onClick.AddListener(() => this.OnAddClick());
        this.itemTemplate.onClick.AddListener(() => this.OnItemClick());
    }

    private void OnAddClick()
    {
        this.viewService.GoTo(Constants.DeviceSearchViewName);
    }

    private void OnItemClick()
    {
        this.viewService.GoTo(Constants.DeviceDetailViewName);
    }
}
