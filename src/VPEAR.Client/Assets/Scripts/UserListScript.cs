using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

public class UserListScript : AbstractView
{
    private IList<GetUserResponse> users = null;

    private void OnEnable()
    {
        var template = this.transform.GetChild(0).gameObject;

        foreach (var device in Enumerable.Range(0, 20))
        {
            var gameObject = Instantiate(template, this.transform);
        }
    }
}
