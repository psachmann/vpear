// <copyright file="DeviceController.cs" company="Patrick Sachmann">
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public DeviceController(
            ILogger<DeviceController> logger,
            IDeviceService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <param name="status">The device status.</param>
        /// <returns>List of devices.</returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Current device information.", typeof(Container<GetDeviceResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnGetAsync([FromQuery] DeviceStatus? status)
        {
            this.logger.LogDebug("{@Status}", status);

            var result = await this.service.GetAsync(status);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Updates the device information.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Changes were saved to db and device.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutDeviceRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);

            var result = await this.service.PutAsync(id, request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Searches for devices. The address and mask are IPv4 addresses and look like 10.0.0.1.
        /// The mask is used to calculate the search range. The supported masks are 255.0.0.0 ... 255.255.255.0.
        /// For example the request address: 10.0.0.0 with mask: 255.255.255.0 will search all address
        /// from 10.0.0.1 to 10.0.0.254. (network address and broadcast address will be excluded)
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Searching for devices.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The address or subnet mask are not IPv4 addresses.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        public async Task<IActionResult> OnPostAsync([FromBody, Required] PostDeviceRequest request)
        {
            var result = await this.service.PostAsync(request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Deletes the given device.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpDelete]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Device was deleted.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Device is archived or recording.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnDeleteAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = await this.service.DeleteAsync(id);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }
    }
}
