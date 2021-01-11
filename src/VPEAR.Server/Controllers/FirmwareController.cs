// <copyright file="FirmwareController.cs" company="Patrick Sachmann">
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
    /// Device firmware management and information.
    /// </summary>
    [ApiController]
    [Route(Routes.FirmwareRoute)]
    public class FirmwareController : Controller
    {
        private readonly ILogger<FirmwareController> logger;
        private readonly IFirmwareService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirmwareController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public FirmwareController(
            ILogger<FirmwareController> logger,
            IFirmwareService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the device firmware information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>The device firmware information.</returns>
        [HttpGet]
        [Authorize]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Current device firmware information.", typeof(GetFirmwareResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = await this.service.GetAsync(id);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Updates the device firmware information.
        /// </summary>
        /// <remarks>
        /// TODO: describe the update process more.
        /// </remarks>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Authorize(Roles = Roles.AdminRole)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Firmware information were saved to device and db.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutFirmwareRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);

            var result = await this.service.PutAsync(id, request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }
    }
}
