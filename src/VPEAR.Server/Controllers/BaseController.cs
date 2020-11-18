// <copyright file="BaseController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// The base controller to check the availability.
    /// </summary>
    [ApiController]
    [Route(Routes.BaseRoute)]
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns Http Status 200, if the server is availble.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void OnGet()
        {
        }
    }
}
