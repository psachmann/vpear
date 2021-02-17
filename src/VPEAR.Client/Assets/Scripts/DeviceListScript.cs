using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceListScript : AbstractView
{
    #region Unity

    [SerializeField] private GameObject content;
    [SerializeField] private Button addButton = null;
    [SerializeField] private Button itemTemplate = null;

    private void Start()
    {
        this.addButton.onClick.AddListener(() => this.OnAddClick());
        // this.itemTemplate.gameObject.SetActive(false);
    }

    #endregion Unity

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        var args = eventArgs as NavigateEventArgs<Null>;

        if (args != null && args.To == this)
        {
            // this.Load();
        }
    }

    private void OnAddClick()
    {
        this.viewService.GoTo(Constants.DeviceSearchViewName);
    }

    private void OnItemClick(GetDeviceResponse device)
    {
        this.viewService.GoTo(Constants.DeviceDetailViewName, device);
    }

    private async void Load(DeviceStatus? deviceStatus = default)
    {
        Logger.Debug("Loading devices...");

        foreach (var button in this.content.GetComponentsInChildren<Button>(false))
        {
            button.gameObject.SetActive(false);
            Destroy(button);
        }

        var container = await Client.GetDevicesAsync(deviceStatus);

        if (container != null)
        {
            foreach (var item in container.Items)
            {
                var temp = Instantiate(this.itemTemplate, this.itemTemplate.transform.parent);

                temp.GetComponent<Text>().text = $"Address: {item.Address}\tStatus: {item.Status}";
                temp.onClick.AddListener(() => this.OnItemClick(item));
                temp.gameObject.SetActive(true);
            }
        }
        else
        {
            this.viewService.popup.Show("Loading failed.", Client.ErrorMessage, () => this.viewService.popup.Hide());
        }

        Logger.Information("Loaded devices.");
    }
}
