// <copyright file="WifiController.cs" company="Patrick Sachmann">
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
    /// Device wifi management and information.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route(Routes.WifiRoute)]
    public class WifiController : Controller
    {
        private readonly ILogger<WifiController> logger;
        private readonly IWifiService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WifiController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public WifiController(
            ILogger<WifiController> logger,
            IWifiService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the device wifi information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>The current device wifi information.</returns>
        [HttpGet]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current wifi information for the device.", typeof(GetWifiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        public IActionResult OnGet([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = this.service.GetAsync(id);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }

        /// <summary>
        /// Updates the device wifi information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Saved wifi information to device and database.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutWifiRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);

            var result = await this.service.PutAsync(id, request);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }
    }
}
