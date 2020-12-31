// <copyright file="SensorController.cs" company="Patrick Sachmann">
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
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// Device sensor information and recorded data.
    /// </summary>
    [ApiController]
    [Authorize]
    public class SensorController : Controller
    {
        private readonly ILogger<SensorController> logger;
        private readonly ISensorService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public SensorController(ILogger<SensorController> logger, ISensorService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the device sensor information.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <returns>Device sensor information.</returns>
        [HttpGet]
        [Route(Routes.SensorsRoute)]
        [SwaggerResponse(StatusCodes.Status200OK, "Device sensor information.", typeof(Container<GetSensorResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorMessages))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        public IActionResult OnGetSensors([FromQuery, Required] Guid id)
        {
            this.logger.LogDebug("{@Device}", id);

            var result = this.service.GetSensorsAsync(id);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }

        /// <summary>
        /// Gets the recorded data.
        /// </summary>
        /// <param name="id">The device id.</param>
        /// <param name="start">The inclusive start frame (0 is the first recorded frame). If omitted, 0 will be assumed.</param>
        /// <param name="stop">The exclusive stop frame. If omitted, the last recorded frame will be assumed.</param>
        /// <returns>The recorded data frames.</returns>
        [HttpGet]
        [Route(Routes.FramesRoute)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "Recorded data frames.", typeof(Container<GetFrameResponse>))]
        [SwaggerResponse(StatusCodes.Status206PartialContent, "Frame count is smaller than stop.", typeof(Container<GetFrameResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format or start is greater or equals stop.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Id not found.", typeof(ErrorResponse))]
        public IActionResult OnGetFrames(
            [FromQuery, Required] Guid id,
            [FromQuery] int? start,
            [FromQuery] int? stop)
        {
            this.logger.LogDebug("{@Device}: {@Request}", id, new { Start = start, Stop = stop, });

            var result = this.service.GetFramesAsync(id, start ?? 0, stop ?? 0);

            this.StatusCode(result.StatusCode);

            return result.IsSuccess ? this.Json(result.Value) : this.Json(result.Error);
        }
    }
}
