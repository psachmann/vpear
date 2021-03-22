using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceDetailScript : AbstractView
{
    [SerializeField] private Text _idText;
    [SerializeField] private Text _addressText;
    [SerializeField] private Text _sensorsText;
    [SerializeField] private Toggle _spotFilterToggle;
    [SerializeField] private Toggle _smoothFilterToggle;
    [SerializeField] private Toggle _noiseFilterToggle;
    [SerializeField] private InputField _nameInput;
    [SerializeField] private InputField _frequencyInput;
    [SerializeField] private Dropdown _statusDropdown;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _arButton;

    private IState<DeviceDetailState> _deviceDetailState;

    private void Start()
    {
        _deviceDetailState = s_provider.GetRequiredService<IState<DeviceDetailState>>();
        _deviceDetailState.StateChanged += DeviceDetailStateChanged;
        _frequencyInput.contentType = InputField.ContentType.IntegerNumber;
        _deleteButton.onClick.AddListener(OnDeleteClick);
        _saveButton.onClick.AddListener(OnSaveClick);
        _arButton.onClick.AddListener(OnARClick);

        DeviceDetailStateChanged(this, _deviceDetailState.Value);
    }

    private void OnDestroy()
    {
        _deviceDetailState.StateChanged -= DeviceDetailStateChanged;
    }

    private void DeviceDetailStateChanged(object sender, DeviceDetailState state)
    {
        _idText.text = state.Device.Id;
        _addressText.text = state.Device.Address;
        _sensorsText.text = state.Device.RequiredSensors.ToString();
        _spotFilterToggle.isOn = state.Filters.Spot;
        _smoothFilterToggle.isOn = state.Filters.Smooth;
        _noiseFilterToggle.isOn = state.Filters.Noise;
        _nameInput.text = state.Device.DisplayName;
        _frequencyInput.text = state.Device.Frequency.ToString();
        _statusDropdown.value = (int)state.Device.Status;
    }

    private void OnDeleteClick()
    {
        _dispatcher.Dispatch(new DeleteDeviceAction(_deviceDetailState.Value.Device));
    }

    private void OnSaveClick()
    {
        _dispatcher.Dispatch(new UpdatingDeviceAction(
            _deviceDetailState.Value.Device,
            _deviceDetailState.Value.Filters,
            _nameInput.text,
            int.Parse(_frequencyInput.text),
            (DeviceStatus)_statusDropdown.value,
            _spotFilterToggle.isOn,
            _smoothFilterToggle.isOn,
            _noiseFilterToggle.isOn));
    }

    private void OnARClick()
    {
        _dispatcher.Dispatch(new ChangeSceneAction(Constants.ARSceneId));
    }
}
