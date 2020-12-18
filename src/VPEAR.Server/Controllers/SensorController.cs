// <copyright file="SensorController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    [ApiController]
    public class SensorController : Controller
    {
        [HttpGet]
        [Route(Routes.SensorsRoute)]
        public void OnGetSensors()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPut]
        [Route(Routes.FramesRoute)]
        public void OnGetFrames()
        {
            this.StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
