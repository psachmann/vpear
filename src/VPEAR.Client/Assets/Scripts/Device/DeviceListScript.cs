using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class DeviceListScript : AbstractView
{
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _itemTemplate;

    private IState<DeviceListState> _deviceListState;
    private IState<LoginState> _loginState;

    private void Start()
    {
        _deviceListState = s_provider.GetRequiredService<IState<DeviceListState>>();
        _loginState = s_provider.GetRequiredService<IState<LoginState>>();
        _deviceListState.StateChanged += DeviceListStateChanged;
        _loginState.StateChanged += LoginStateChanged;
        _addButton.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceSearchViewName)));
        _itemTemplate.gameObject.SetActive(false);

        DeviceListStateChanged(this, _deviceListState.Value);
        LoginStateChanged(this, _loginState.Value);
    }

    private void OnDestroy()
    {
        _deviceListState.StateChanged -= DeviceListStateChanged;
        _loginState.StateChanged -= LoginStateChanged;
    }

    private void DeviceListStateChanged(object sender, DeviceListState state)
    {
        foreach (var button in _content.GetComponentsInChildren<Button>(false))
        {
            button.gameObject.SetActive(false);
            Destroy(button);
        }

        foreach (var device in state.Devices)
        {
            var temp = Instantiate(_itemTemplate, _itemTemplate.transform.parent);

            temp.gameObject.SetActive(true);
            temp.GetComponentInChildren<Text>().text =
                !string.IsNullOrEmpty(device.DisplayName) ? $"Name: {device.DisplayName}" : $"Address: {device.Address}";
            temp.onClick.AddListener(() =>
            {
                _dispatcher.Dispatch(new SelectDeviceAction(device));
                _dispatcher.Dispatch(new FetchingDeviceAction(device));
                _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceDetailViewName));
            });
        }
    }

    private void LoginStateChanged(object sender, LoginState state)
    {
        _addButton.gameObject.SetActive(state.IsAdmin);
    }
}
