using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class UserListScript : AbstractView
{
    [SerializeField] private Button itemTemplate = null;
    private IList<GetUserResponse> users = null;

    private void Start()
    {
        this.itemTemplate.onClick.AddListener(() => this.OnItemClick());
    }

    private void OnItemClick()
    {
        this.viewService.GoTo(Constants.UserDetailViewName);
    }

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        if (((NavigateEventArgs)eventArgs).To == this)
        {
            // this.LoadUsersAsync();
        }
    }

    private async void LoadUsersAsync()
    {
        Logger.Debug("Loading users...");

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

        Logger.Information("Loaded users.");
    }
}
