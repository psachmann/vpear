// <copyright file="WifiController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    [ApiController]
    [Route(Routes.WifiRoute)]
    public class WifiController : Controller
    {
        [HttpGet]
        public void OnGet()
        {
            this.Response.StatusCode = StatusCodes.Status501NotImplemented;
        }

        [HttpPut]
        public void OnPut()
        {
            this.Response.StatusCode = StatusCodes.Status501NotImplemented;
        }
    }
}
