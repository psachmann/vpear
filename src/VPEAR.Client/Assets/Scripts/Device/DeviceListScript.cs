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

    private void Start()
    {
        _deviceListState = s_provider.GetRequiredService<IState<DeviceListState>>();
        _deviceListState.StateChanged += DeviceListStateChanged;
        _addButton.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceSearchViewName)));
        _itemTemplate.gameObject.SetActive(false);

        DeviceListStateChanged(this, _deviceListState.Value);
    }

    private void DeviceListStateChanged(object sender, DeviceListState state)
    {
        foreach (var button in _content.GetComponentsInChildren<Button>())
        {
            Destroy(button);
        }

        foreach (var device in state.Devices)
        {
            var temp = Instantiate(_itemTemplate, _content.transform);

            temp.gameObject.SetActive(true);
            temp.GetComponent<Text>().text = $"Name: {device.DisplayName}\tAddress: {device.Address}";
            temp.onClick.AddListener(() =>
            {
                _dispatcher.Dispatch(new SelectDeviceAction(device));
                _dispatcher.Dispatch(new FetchingDeviceAction(device));
                _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceDetailViewName));
            });
        }
    }

    public override void Show()
    {
        _dispatcher.Dispatch(new FetchingDevicesAction(_deviceListState.Value.Status));

        base.Show();
    }
}
