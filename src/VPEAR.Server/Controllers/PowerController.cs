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
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// Device power information.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route(Routes.PowerRoute)]
    public class PowerController : Controller
    {
        private readonly ILogger<PowerController> logger;
        private readonly IPowerService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public PowerController(ILogger<PowerController> logger, IPowerService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the current device power information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>The current device power information.</returns>
        [HttpGet]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current device power information.", typeof(GetPowerResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = await this.service.GetAsync(id);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }
    }
}
