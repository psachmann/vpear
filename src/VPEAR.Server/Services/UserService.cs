// <copyright file="UserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IUserService"/> interface.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> roles;
        private readonly UserManager<IdentityUser> users;
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="roles">The role manager.</param>
        /// <param name="users">The user manager.</param>
        /// <param name="logger">The service logger.</param>
        public UserService(
            RoleManager<IdentityRole> roles,
            UserManager<IdentityUser> users,
            ILogger<UserController> logger)
        {
            this.roles = roles;
            this.users = users;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> DeleteAsync(string id)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var user = await this.users.FindByIdAsync(id);

            if (user == null)
            {
                status = HttpStatusCode.NotFound;
                message = ErrorMessages.UserNotFound;
            }
            else if ((await this.users.GetRolesAsync(user)).Contains(Roles.AdminRole)
                && (await this.users.GetUsersInRoleAsync(Roles.AdminRole)).Count == 1)
            {
                status = HttpStatusCode.Forbidden;
                message = ErrorMessages.LastAdminError;
            }
            else
            {
                if ((await this.users.DeleteAsync(user)).Succeeded)
                {
                    return new Result<Null>(HttpStatusCode.OK);
                }
            }

            return new Result<Null>(status, message);
        }

        /// <inheritdoc/>
        public async Task<Result<Container<GetUserResponse>>> GetAsync(string? role = null)
        {
            if (role == null)
            {
                var admins = await this.users.GetUsersInRoleAsync(Roles.AdminRole);
                var users = await this.users.GetUsersInRoleAsync(Roles.UserRole);
                var payload = new Container<GetUserResponse>();

                foreach (var admin in admins)
                {
                    payload.Items.Add(new GetUserResponse()
                    {
                        Name = admin.UserName,
                        Id = admin.Id,
                        IsVerified = admin.EmailConfirmed,
                        Roles = Roles.AllRoles,
                    });
                }

                foreach (var user in users)
                {
                    payload.Items.Add(new GetUserResponse()
                    {
                        Name = user.UserName,
                        Id = user.Id,
                        IsVerified = user.EmailConfirmed,
                        Roles = new List<string>() { Roles.UserRole, },
                    });
                }

                return new Result<Container<GetUserResponse>>(HttpStatusCode.OK, payload);
            }
            else
            {
                var users = await this.users.GetUsersInRoleAsync(role);
                var payload = new Container<GetUserResponse>();

                foreach (var user in users)
                {
                    payload.Items.Add(new GetUserResponse()
                    {
                        Name = user.UserName,
                        Id = user.Id,
                        IsVerified = user.EmailConfirmed,
                        Roles = await this.users.GetRolesAsync(user),
                    });
                }

                return new Result<Container<GetUserResponse>>(HttpStatusCode.OK, payload);
            }
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PostRegisterAsync(PostRegisterRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = ErrorMessages.InternalServerError;
            var existingUser = await this.users.FindByNameAsync(request.Name);

            if (existingUser == null)
            {
                var user = new IdentityUser()
                {
                    UserName = request.Name,
                    SecurityStamp = DateTimeOffset.UtcNow.ToString(),
                };

                if (request.IsAdmin)
                {
                    await this.users.AddToRolesAsync(user, Roles.AllRoles);
                }
                else
                {
                    await this.users.AddToRoleAsync(user, Roles.UserRole);
                }

                if ((await this.users.CreateAsync(user, request.Password)).Succeeded)
                {
                    return new Result<Null>(HttpStatusCode.OK);
                }
            }
            else
            {
                status = HttpStatusCode.Conflict;
                message = ErrorMessages.UserNameAlreadyUsed;
            }

            return new Result<Null>(status, message);
        }

        /// <inheritdoc/>
        public async Task<Result<Null>> PutAsync(string id, PutUserRequest request)
        {
            var user = await this.users.FindByIdAsync(id);

            if (user == null)
            {
                return new Result<Null>(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);
            }

            if (request.NewPassword != null && request.OldPassword != null
                && !(await this.users.ChangePasswordAsync(user, request.OldPassword, request.NewPassword)).Succeeded)
            {
                return new Result<Null>(HttpStatusCode.InternalServerError, ErrorMessages.InternalServerError);
            }

            if (request.IsVerified)
            {
                user.EmailConfirmed = request.IsVerified;

                if (!(await this.users.UpdateAsync(user)).Succeeded)
                {
                    return new Result<Null>(HttpStatusCode.InternalServerError, ErrorMessages.InternalServerError);
                }
            }

            return new Result<Null>(HttpStatusCode.OK);
        }

        /// <inheritdoc/>
        public async Task<Result<PutLoginResponse>> PutLoginAsync(PutLoginRequest request)
        {
            var user = await this.users.FindByNameAsync(request.Name);

            if (user == null)
            {
                return new Result<PutLoginResponse>(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);
            }

            if (!user.EmailConfirmed)
            {
                return new Result<PutLoginResponse>(HttpStatusCode.Forbidden, ErrorMessages.UserNotVerified);
            }

            if (await this.users.CheckPasswordAsync(user, request.Password))
            {
                var userRoles = await this.users.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Config!.Secret));

                // TODO: add issuer and audience
                // issuer: _configuration["JWT:ValidIssuer"],
                // audience: _configuration["JWT:ValidAudience"],
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(24),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512));
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                var payload = new PutLoginResponse()
                {
                    ExpiresAt = token.ValidTo,
                    Token = tokenString,
                };

                await this.users.SetAuthenticationTokenAsync(user, JwtBearerDefaults.AuthenticationScheme, string.Empty, tokenString);

                return new Result<PutLoginResponse>(HttpStatusCode.OK, payload);
            }
            else
            {
                return new Result<PutLoginResponse>(HttpStatusCode.Forbidden, ErrorMessages.InvalidPassword);
            }
        }
    }
}
