// <copyright file="FirmwareController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using FluentValidation;
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
    /// Device firmware management and information.
    /// </summary>
    [ApiController]
    [Route(Routes.FirmwareRoute)]
    public class FirmwareController : Controller
    {
        private readonly ILogger<FirmwareController> logger;
        private readonly IFirmwareService service;
        private readonly IValidator<PutFirmwareRequest> putFirmwareValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirmwareController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        /// <param name="putFirmwareValidator">The put firmware request validator.</param>
        public FirmwareController(
            ILogger<FirmwareController> logger,
            IFirmwareService service,
            IValidator<PutFirmwareRequest> putFirmwareValidator)
        {
            this.logger = logger;
            this.service = service;
            this.putFirmwareValidator = putFirmwareValidator;
        }

        /// <summary>
        /// Gets the device firmware information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>The device firmware information.</returns>
        [HttpGet]
        [Authorize]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Current device firmware information.", typeof(GetFirmwareResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        public async Task<IActionResult> OnGet([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var response = await this.service.GetAsync(id);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }

        /// <summary>
        /// Updates the device firmware information.
        /// </summary>
        /// <remarks>
        /// TODO: describe the update process more.
        /// </remarks>
        /// <param name="id">The device id.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Authorize(Roles = Roles.AdminRole)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Firmware information were saved to db and device.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Started update process.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(object))]
        public async Task<IActionResult> OnPut([FromQuery, Required] Guid id, [FromBody, Required] PutFirmwareRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);
            this.putFirmwareValidator.ValidateAndThrow(request);

            var response = await this.service.PutAsync(id, request);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }
    }
}
