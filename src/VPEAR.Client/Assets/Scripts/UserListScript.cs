using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class UserListScript : AbstractView
{
    #region Unity

    [SerializeField] private GameObject content = null;
    [SerializeField] private Button itemTemplate = null;

    private void Start()
    {
        this.itemTemplate.onClick.AddListener(() => this.OnItemClick(default));
        // this.itemTemplate.gameObject.SetActive(false);
    }

    #endregion Unity

    public override void NavigateEventHandler(object sender, EventArgs eventArgs)
    {
        var args = eventArgs as NavigateEventArgs<Null>;

        if (args != null && args.To == this)
        {
            // this.Load();
        }
    }

    private void OnItemClick(GetUserResponse user)
    {
        this.viewService.GoTo(Constants.UserDetailViewName, user);
    }

    private async void Load()
    {
        Logger.Debug("Loading users...");

        foreach (var button in this.content.GetComponentsInChildren<Button>(false))
        {
            button.gameObject.SetActive(false);
            Destroy(button);
        }

        var container = await Client.GetUsersAsync();

        if (container != null)
        {
            foreach (var user in container.Items)
            {
                var temp = Instantiate(this.itemTemplate, this.itemTemplate.transform.parent);

                temp.GetComponent<Text>().text = user.Name;
                temp.onClick.AddListener(() => this.OnItemClick(user));
                temp.gameObject.SetActive(true);
            }
        }
        else
        {
            this.viewService.popup.Show("Loading failed.", Client.ErrorMessage, () => this.viewService.popup.Hide());
        }

        Logger.Information("Loaded users.");
    }
}
