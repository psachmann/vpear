using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceDetailScript : AbstractView
{
    private GetDeviceResponse device = null;
    private GetFiltersResponse filters = null;

    #region Unity

    [SerializeField] private Text idText = null;
    [SerializeField] private Text addressText = null;
    [SerializeField] private Text sensorsText = null;
    [SerializeField] private Toggle spotFilterToggle = null;
    [SerializeField] private Toggle smoothFilterToggle = null;
    [SerializeField] private Toggle noiseFilterToggle = null;
    [SerializeField] private InputField nameInput = null;
    [SerializeField] private InputField frequencyInput = null;
    [SerializeField] private Dropdown statusDropdown = null;
    [SerializeField] private Button saveButton = null;

    private void Start()
    {
        this.frequencyInput.contentType = InputField.ContentType.IntegerNumber;
        this.saveButton.onClick.AddListener(() => this.OnSaveClick());
    }

    #endregion Unity

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        if (eventArgs is NavigateEventArgs<GetDeviceResponse> args && args.To == this)
        {
            this.Load(args.Payload);
        }
    }

    private async void Load(GetDeviceResponse device)
    {
        Logger.Debug("Loading device information...");

        var filters = await Client.GetFiltersAsync(device.Id);

        this.device = device;
        this.filters = filters;
        this.idText.text = device.Id;
        this.addressText.text = device.Address;
        this.sensorsText.text = device.RequiredSensors.ToString();
        this.spotFilterToggle.isOn = filters.Spot;
        this.smoothFilterToggle.isOn = filters.Smooth;
        this.noiseFilterToggle.isOn = filters.Noise;
        this.nameInput.text = device.DisplayName;
        this.frequencyInput.text = device.Frequency.ToString();
        this.statusDropdown.value = (int)device.Status;

        if (device.Status == DeviceStatus.Archived)
        {
            this.statusDropdown.enabled = false;
        }

        Logger.Debug("Loaded device information.");
    }

    private async void OnSaveClick()
    {
        if ((this.filters.Noise != this.noiseFilterToggle.isOn
            || this.filters.Smooth != this.smoothFilterToggle.isOn
            || this.filters.Spot != this.spotFilterToggle.isOn)
            && !await Client.PutFiltersAsync(this.device.Id, this.spotFilterToggle.isOn, this.smoothFilterToggle.isOn, this.noiseFilterToggle.isOn))
        {
            Helpers.ShowClientError(Client, this.viewService, this.OnSaveFailure);

            return;
        }

        if ((this.device.DisplayName != this.nameInput.text
            || this.device.Frequency.ToString() != this.frequencyInput.text
            || (int)this.device.Status != this.statusDropdown.value)
            && !await Client.PutDeviceAsync(this.device.Id, this.nameInput.text, int.Parse(this.frequencyInput.text), null, (DeviceStatus)this.statusDropdown.value))
        {
            Helpers.ShowClientError(Client, this.viewService, this.OnSaveFailure);

            return;
        }
    }

    private void OnSaveFailure()
    {
        this.spotFilterToggle.isOn = filters.Spot;
        this.smoothFilterToggle.isOn = filters.Smooth;
        this.noiseFilterToggle.isOn = filters.Noise;
        this.frequencyInput.text = device.Frequency.ToString();
        this.statusDropdown.value = (int)device.Status;
    }
}
