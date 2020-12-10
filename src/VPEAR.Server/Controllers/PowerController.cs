// <copyright file="PowerController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// The endpoint handels power management information for a specific device.
    /// </summary>
    [ApiController]
    [Route(Routes.PowerRoute)]
    public class PowerController : Controller
    {
        private readonly ILogger<PowerController> logger;
        private readonly IPowerService service;

        public PowerController(ILogger<PowerController> logger, IPowerService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the current power information for the specific device.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>The current device power information.</returns>
        [HttpGet]
        [Authorize]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current power information for the device.", typeof(PowerResponse))]
        [SwaggerResponse(StatusCodes.Status202Accepted, "The device is not reachable.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The request has the wrong format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request is not authorized.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            var response = await this.service.GetAsync(id);

            this.Response.StatusCode = response.StatusCode;

            return response.Payload;
        }
    }
}
