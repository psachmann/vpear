// <copyright file="DeviceController.cs" company="Patrick Sachmann">
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
    /// Basic device management and information.
    /// </summary>
    [Authorize]
    [Route(Routes.DeviceRoute)]
    [Produces(Defaults.DefaultResponseType)]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDeviceService service;
        private readonly IValidator<PutDeviceRequest> putDeviceValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        /// <param name="putDeviceValidator">The put device request validator.</param>
        public DeviceController(
            ILogger<DeviceController> logger,
            IDeviceService service,
            IValidator<PutDeviceRequest> putDeviceValidator)
        {
            this.logger = logger;
            this.service = service;
            this.putDeviceValidator = putDeviceValidator;
        }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>The current device information.</returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Current device information.", typeof(GetDeviceResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var response = await this.service.GetAsync(id);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }

        /// <summary>
        /// Updates the device information.
        /// </summary>
        /// <remarks>
        /// TODO: Describe the request data.
        /// </remarks>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, "Changes were saved to db and device.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Device is currently recording.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(object))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutDeviceRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);
            this.putDeviceValidator.ValidateAndThrow(request);

            var response = await this.service.PutAsync(id, request);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }

        /// <summary>
        /// Searches for devices.
        /// </summary>
        /// <remarks>
        /// TODO: Describe the request data.
        /// </remarks>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status102Processing, "Searching for devices.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        public async Task<IActionResult> OnPostAsync([FromBody, Required] PostDeviceRequest request)
        {
            var response = await this.service.PostAsync(request);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }

        /// <summary>
        /// The admin can delete a device.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpDelete]
        [Authorize(Roles = Roles.AdminRole)]
        [SwaggerResponse(StatusCodes.Status200OK, "Device was deleted.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Device is currently recording.", typeof(object))]
        public async Task<IActionResult> OnDeleteAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var response = await this.service.DeleteAsync(id);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }
    }
}
