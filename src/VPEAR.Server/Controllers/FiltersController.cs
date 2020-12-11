// <copyright file="FiltersController.cs" company="Patrick Sachmann">
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
    /// Filter management for a specific device.
    /// </summary>
    [ApiController]
    [Route(Routes.FiltersRoute)]
    public class FiltersController : Controller
    {
        private readonly ILogger<FiltersController> logger;
        private readonly IFiltersService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public FiltersController(ILogger<FiltersController> logger, IFiltersService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the filters for the specific device.
        /// </summary>
        /// <param name="id">The device id as hex string. Format: XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</param>
        /// <returns>The current filters for the device.</returns>
        [HttpGet]
        [Authorize]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current filters for the device.", typeof(GetFiltersResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request is not authorized.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The id was not found.", typeof(StatusCodes))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Request}", id);

            var response = await this.service.GetAsync(id);

            this.Response.StatusCode = response.StatusCode;

            return this.Json(response.Payload);
        }

        /// <summary>
        /// Sets the filters for the specific device.
        /// </summary>
        /// <param name="id">The device id as hex string. Format: XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</param>
        /// <param name="request">Request data as json string.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Authorize]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Saved filters to device and database.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Saved filters to device and database, but recording is stopped.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request is not authorized.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The id was not found.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status410Gone, "The device is archived.", typeof(StatusCodes))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "The device is not reachable.", typeof(StatusCodes))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody] PutFiltersRequest request)
        {
            this.logger.LogDebug("{@Request}", request);

            var response = await this.service.PutAsync(id, request);

            this.Response.StatusCode = response.StatusCode;

            return this.Json(response.Payload);
        }
    }
}
