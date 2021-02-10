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
}
