// <copyright file="UserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly ILogger<UserController> logger;
        private readonly RoleManager<IdentityRole> roles;
        private readonly UserManager<IdentityUser> users;

        public UserService(
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roles,
            UserManager<IdentityUser> users)
        {
            this.logger = logger;
            this.roles = roles;
            this.users = users;
        }

        /// <inheritdoc/>
        public async Task<Response> DeleteAsync(string id)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var user = await this.users.FindByIdAsync(id);

            if (user == null)
            {
                status = HttpStatusCode.NotFound;
                payload = null;
            }
            else if ((await this.users.GetRolesAsync(user)).Contains(Roles.AdminRole)
                && (await this.users.GetUsersInRoleAsync(Roles.AdminRole)).Count == 1)
            {
                status = HttpStatusCode.Forbidden;
                payload = new ErrorResponse(status, ErrorMessages.LastAdminError);
            }
            else
            {
                if ((await this.users.DeleteAsync(user)).Succeeded)
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }
            }

            return new Response(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> GetAsync(string? id = null, string? role = null)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);

            if (id != null && role == null
                && (await this.users.FindByIdAsync(id)) != null)
            {
                var user = await this.users.FindByIdAsync(id);
                var wrapper = new GetUserResponse()
                {
                    DisplayName = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    IsVerified = user.EmailConfirmed,
                    Roles = await this.users.GetRolesAsync(user),
                };

                status = HttpStatusCode.OK;
                payload = new Container<GetUserResponse>()
                {
                    Items = new List<GetUserResponse>() { wrapper, },
                    Start = 0,
                    Stop = 1,
                };
            }
            else if (id == null && role != null
                && (await this.users.GetUsersInRoleAsync(role)).Count >= 1)
            {
                var users = await this.users.GetUsersInRoleAsync(role);
                var wrapper = new List<GetUserResponse>();

                users.ToList().ForEach(async u =>
                {
                    wrapper.Add(new GetUserResponse()
                    {
                        DisplayName = u.UserName,
                        Email = u.Email,
                        Id = u.Id,
                        IsVerified = u.EmailConfirmed,
                        Roles = await this.users.GetRolesAsync(u),
                    });
                });

                status = HttpStatusCode.OK;
                payload = new Container<GetUserResponse>()
                {
                    Items = wrapper,
                    Start = 0,
                    Stop = wrapper.Count - 1,
                };
            }
            else
            {
                // no id or role provided
                status = HttpStatusCode.NotFound;
                payload = null;
            }

            return new Response(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> PostRegisterAsync(PostRegisterRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var existingUser = await this.users.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                var user = new IdentityUser()
                {
                    Email = request.Email,
                    SecurityStamp = DateTimeOffset.UtcNow.ToString(),
                    UserName = request.DisplayName ?? request.Email,
                };

                if ((await this.users.CreateAsync(user, request.Password)).Succeeded)
                {
                    status = HttpStatusCode.OK;
                    payload = null;
                }

                if (request.IsAdmin)
                {
                    await this.CreateAdminAsync(user);
                }
            }
            else
            {
                status = HttpStatusCode.Conflict;
                payload = new ErrorResponse(status, ErrorMessages.UserEmailAlreadyUsed);
            }

            return new Response(status, payload);
        }

        /// <inheritdoc/>
        public async Task<Response> PutAsync(string id, PutUserRequest request)
        {
            var user = await this.users.FindByIdAsync(id);

            if (user == null)
            {
                return new Response(HttpStatusCode.NotFound);
            }

            if (request.OldPassword != null && request.NewPassword != null
                && !(await this.users.ChangePasswordAsync(user, request.OldPassword, request.NewPassword)).Succeeded)
            {
                return new Response(HttpStatusCode.InternalServerError);
            }

            if (request.IsVerified)
            {
                if (!(await this.users.AddToRolesAsync(user, request.Roles)).Succeeded)
                {
                    return new Response(HttpStatusCode.InternalServerError);
                }
            }

            return new Response(HttpStatusCode.OK);
        }

        /// <inheritdoc/>
        public async Task<Response> PutLoginAsync(PutLoginRequest request)
        {
            var status = HttpStatusCode.InternalServerError;
            dynamic? payload = new ErrorResponse(status, ErrorMessages.InternalServerError);
            var user = await this.users.FindByEmailAsync(request.Email);

            if (user == null || user.EmailConfirmed)
            {
                status = HttpStatusCode.NotFound;
                payload = null;
            }
            else if (await this.users.CheckPasswordAsync(user, request.Password))
            {
                status = HttpStatusCode.Unauthorized;
                payload = null;
            }
            else
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
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                status = HttpStatusCode.OK;
                payload = new PutLoginResponse()
                {
                    ExpiresAt = token.ValidTo.ToString(Schemas.TimeSchema),
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };
            }

            return new Response(status, payload);
        }

        private async Task CreateAdminAsync(IdentityUser user)
        {
            if (!(await this.roles.RoleExistsAsync(Roles.AdminRole)))
            {
                await this.roles.CreateAsync(new IdentityRole(Roles.AdminRole));
            }

            if (!(await this.roles.RoleExistsAsync(Roles.UserRole)))
            {
                await this.roles.CreateAsync(new IdentityRole(Roles.UserRole));
            }

            if (await this.roles.RoleExistsAsync(Roles.AdminRole))
            {
                await this.users.AddToRoleAsync(user, Roles.AdminRole);
            }
        }
    }
}
