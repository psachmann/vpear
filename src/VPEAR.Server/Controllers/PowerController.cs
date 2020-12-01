// <copyright file="PowerController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    [ApiController]
    [Route(Routes.PowerRoute)]
    public class PowerController : Controller
    {
        private readonly ILogger<PowerController> logger;
        private readonly IPowerService service;

        public PowerController(ILogger<PowerController> logger, IPowerService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> OnGetAsync([FromQuery] string id)
        {
            var response = await this.service.GetAsync(new Guid(id));

            this.Response.StatusCode = response.StatusCode;

            return this.Json(response.Payload);
        }
    }
}
