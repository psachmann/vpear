using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceDetailScript : AbstractView
{
    [SerializeField] private Text idText;
    [SerializeField] private Text addressText;
    [SerializeField] private Text sensorsText;
    [SerializeField] private Toggle spotFilterToggle;
    [SerializeField] private Toggle smoothFilterToggle;
    [SerializeField] private Toggle noiseFilterToggle;
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField frequencyInput;
    [SerializeField] private Dropdown statusDropdown;
    [SerializeField] private Button saveButton;

    private void Start()
    {
        // this.frequencyInput.contentType = InputField.ContentType.IntegerNumber;
        // this.saveButton.onClick.AddListener(() => this.OnSaveClick());
    }
}
