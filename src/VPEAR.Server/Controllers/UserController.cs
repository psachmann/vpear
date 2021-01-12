// <copyright file="UserController.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Controllers
{
    /// <summary>
    /// User management and information.
    /// </summary>
    [ApiController]
    [Route(Routes.UsersRoute)]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">The controller logger.</param>
        /// <param name="service">The controller service.</param>
        public UserController(
            ILogger<UserController> logger,
            IUserService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// The admin can search for user based on id or role.
        /// A GET request without any query parameters returns all users.
        /// </summary>
        /// <param name="role">The user role. Should be 'admin' or 'user'.</param>
        /// <returns>A list of found users.</returns>
        [HttpGet]
        [Authorize(Roles = Roles.AdminRole)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "The user or users were found.", typeof(Container<GetUserResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is unauthorized.", typeof(Null))]
        public async Task<IActionResult> OnGetAsync([FromQuery] string? role)
        {
            this.logger.LogDebug("{@Role}", role);

            var result = await this.service.GetAsync(role);

            return this.StatusCode(result.StatusCode, result.Value);
        }

        /// <summary>
        /// The admin can update a user.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPut]
        [Authorize(Roles = Roles.AdminRole)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User was updated and saved to db.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is not authorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No user found.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutAsync([FromQuery, Required] string name, [FromQuery, Required] PutUserRequest request)
        {
            this.logger.LogDebug("{@User}: {@Request}", name, request);

            var result = await this.service.PutAsync(name, request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// The admin can delete a user.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpDelete]
        [Authorize(Roles = Roles.AdminRole)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User was deleted from db.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Request is not authorized.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Last admin will not be deleted.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No user found.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnDeleteAsync([FromQuery, Required] string name = "")
        {
            this.logger.LogDebug("{@User}", name);

            var result = await this.service.DeleteAsync(name);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>Http status code, which indicates the operation result.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.RegisterRoute)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User was registered.", typeof(Null))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Email is already used.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPostRegisterAsync([FromBody, Required] PostRegisterRequest request)
        {
            this.logger.LogDebug("@{Request}", request);

            var result = await this.service.PostRegisterAsync(request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }

        /// <summary>
        /// Generates token to access the endpoints, which requires authorization.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>The authorization token and the date, when the token expires.</returns>
        [HttpPut]
        [AllowAnonymous]
        [Route(Routes.LoginRoute)]
        [Produces(Defaults.DefaultResponseType)]
        [SwaggerResponse(StatusCodes.Status200OK, "User was logged in.", typeof(PutLoginResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Wrong request format.", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No user found.", typeof(ErrorResponse))]
        public async Task<IActionResult> OnPutLoginAsync([FromBody, Required] PutLoginRequest request)
        {
            this.logger.LogDebug("{@Request}", request);

            var result = await this.service.PutLoginAsync(request);

            return result.IsSuccess ? this.StatusCode(result.StatusCode, result.Value) : this.StatusCode(result.StatusCode, result.Error);
        }
    }
}
