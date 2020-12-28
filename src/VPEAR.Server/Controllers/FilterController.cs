// <copyright file="FilterController.cs" company="Patrick Sachmann">
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
    /// Device filters management and information.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route(Routes.FilterRoute)]
    public class FilterController : Controller
    {
        private readonly ILogger<FilterController> logger;
        private readonly IFilterService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public FilterController(ILogger<FilterController> logger, IFilterService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the device filters.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <returns>The current filters for the device.</returns>
        [HttpGet]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The current device filters.", typeof(GetFiltersResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is not authorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnGetAsync([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = await this.service.GetAsync(id);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }

        /// <summary>
        /// Sets the device filters.
        /// </summary>
        /// <param name="id">The device id as 32 digit hex string.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Saved filters to device and database.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status410Gone, "Device is archived.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Device is not reachable.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] Guid id, [FromBody, Required] PutFiltersRequest request)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, request);

            var result = await this.service.PutAsync(id, request);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }
    }
}
