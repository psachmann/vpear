// <copyright file="UserController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// User management and information.
    /// </summary>
    [ApiController]
    [Route(Routes.UsersRoute)]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }

        [HttpPut]
        public async Task<IActionResult> OnPutAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }

        [HttpDelete]
        public async Task<IActionResult> OnDeleteAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }

        [HttpPost]
        [Route(Routes.RegisterRoute)]
        public async Task<IActionResult> OnPostRegisterAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }

        [HttpPut]
        [Route(Routes.LoginRoute)]
        public async Task<IActionResult> OnPutLoginAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }

        [HttpPut]
        [Route(Routes.LogoutRoute)]
        public async Task<IActionResult> OnPutLogoutAsync()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);

            return this.Json(null);
        }
    }
}
