// <copyright file="WifiController.cs" company="Patrick Sachmann">
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
    /// Device wifi management and information.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route(Routes.WifiRoute)]
    public class WifiController : Controller
    {
        private readonly ILogger<WifiController> logger;
        private readonly IWifiService service;
        private readonly IValidator<PutWifiRequest> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WifiController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        /// <param name="validator">The PUT request validator.</param>
        public WifiController(
            ILogger<WifiController> logger,
            IWifiService service,
            IValidator<PutWifiRequest> validator)
        {
            this.logger = logger;
            this.service = service;
            this.validator = validator;
        }

        /// <summary>
        /// Gets the device wifi information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>The current device wifi information.</returns>
        [HttpGet]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current wifi information for the device.", typeof(GetWifiResponse))]
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
        /// Updates the device wifi information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Saved wifi information to device and database.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(object))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(object))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutWifiRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);
            this.validator.ValidateAndThrow(request);

            var response = await this.service.PutAsync(id, request);

            this.StatusCode(response.StatusCode);

            return this.Json(response.Payload);
        }
    }
}
