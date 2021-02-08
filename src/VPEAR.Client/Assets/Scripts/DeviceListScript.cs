using System.Collections.Generic;
using System.Linq;
using VPEAR.Core.Wrappers;

public class DeviceListScript : AbstractView
{
    private IList<GetDeviceResponse> devices = null;

    private void OnEnable()
    {
        var template = this.transform.GetChild(0).gameObject;

        foreach (var device in Enumerable.Range(0, 10))
        {
            var gameObject = Instantiate(template, this.transform);
        }

        Destroy(template);
    }

    private void GoBack()
    {
        this.viewService.GoBack();
    }
}
