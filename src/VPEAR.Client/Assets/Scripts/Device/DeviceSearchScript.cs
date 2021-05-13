using UnityEngine;
using UnityEngine.UI;

public class DeviceSearchScript : AbstractView
{
    [SerializeField] private InputField _addressInput;
    [SerializeField] private InputField _maskInput;
    [SerializeField] private Button _searchButton;

    private void Start()
    {
        _addressInput.onValueChanged.AddListener(IsSearchEnabled);
        _maskInput.onValueChanged.AddListener(IsSearchEnabled);
        _searchButton.onClick.AddListener(() => _dispatcher.Dispatch(new DeviceSearchAction(_addressInput.text, _maskInput.text)));

        IsSearchEnabled();
    }

    private void IsSearchEnabled(string _ = default)
    {
        if (string.IsNullOrEmpty(_addressInput.text) || string.IsNullOrEmpty(_maskInput.text))
        {
            _searchButton.enabled = false;
        }
        else
        {
            _searchButton.enabled = true;
        }
    }
}
