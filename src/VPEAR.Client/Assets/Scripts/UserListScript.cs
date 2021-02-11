using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class UserListScript : AbstractView
{
    private IList<GetUserResponse> users = null;

    private void OnEnable()
    {
        var buttons = new List<Button>();
        this.GetComponentsInChildren(buttons);
        var button = buttons.First(button => string.Equals(button.name, Constants.UserListItemName));

        foreach (var device in Enumerable.Range(0, 20))
        {
            var gameObject = Instantiate(button, button.transform.parent);
        }

        Destroy(button);
    }

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        if (((NavigateEventArgs)eventArgs).To == this)
        {
            this.LoadUsersAsync();
        }
    }

    private async void LoadUsersAsync()
    {
        Logger.Debug("Loading users");

        var buttons = new List<Button>();
        this.GetComponentsInChildren(buttons);
        var button = buttons.First(button => string.Equals(button.name, Constants.UserListItemName));
        var container = await Client.GetUsersAsync();
        this.users = container.Items;

        foreach (var user in this.users)
        {
            var temp = Instantiate(button, this.transform);
            temp.GetComponent<Text>().text = user.Name;
        }

        Logger.Information("Loaded users");
    }
}
