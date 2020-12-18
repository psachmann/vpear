// <copyright file="DeviceController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    [ApiController]
    [Route(Routes.DeviceRoute)]
    public class DeviceController : Controller
    {
        [HttpGet]
        public void OnGetAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPut]
        public void OnPutAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public void OnPostAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpDelete]
        public void OnDeleteAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
