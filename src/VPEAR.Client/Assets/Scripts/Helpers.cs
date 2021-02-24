using UnityEngine.Events;
using VPEAR.Core;

internal static class Helpers
{
    public static void ShowClientError(VPEARClient client, ViewService viewService, UnityAction action = default)
    {
        var popup = viewService.GetViewByName(Constants.PopupViewName) as PopupScript;

        popup.Show("Request Error", client.ErrorMessage, () =>
        {
            action?.Invoke();
            popup.Hide();
        });
    }
}
