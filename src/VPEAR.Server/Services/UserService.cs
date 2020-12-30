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
                        DisplayName = admin.UserName,
                        Email = admin.Email,
                        Id = admin.Id,
                        IsVerified = admin.EmailConfirmed,
                        Roles = Roles.AllRoles,
                    });
                }

                foreach (var user in users)
                {
                    payload.Items.Add(new GetUserResponse()
                    {
                        DisplayName = user.UserName,
                        Email = user.Email,
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
                        DisplayName = user.UserName,
                        Email = user.Email,
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
            var existingUser = await this.users.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                var user = new IdentityUser()
                {
                    Email = request.Email,
                    SecurityStamp = DateTimeOffset.UtcNow.ToString(),
                    UserName = request.DisplayName ?? request.Email,
                };

                if (request.IsAdmin)
                {
                    await this.CreateAdminAsync(user);
                }

                if ((await this.users.CreateAsync(user, request.Password)).Succeeded)
                {
                    return new Result<Null>(HttpStatusCode.OK);
                }
            }
            else
            {
                status = HttpStatusCode.Conflict;
                message = ErrorMessages.UserEmailAlreadyUsed;
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
            var user = await this.users.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new Result<PutLoginResponse>(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);
            }
            else if (!user.EmailConfirmed)
            {
                return new Result<PutLoginResponse>(HttpStatusCode.Forbidden, ErrorMessages.UserNotVerfied);
            }
            else if (!await this.users.CheckPasswordAsync(user, request.Password))
            {
                return new Result<PutLoginResponse>(HttpStatusCode.Forbidden, ErrorMessages.InvalidPassword);
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

                var payload = new PutLoginResponse()
                {
                    ExpiresAt = token.ValidTo.ToString(Schemas.TimeSchema),
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };

                return new Result<PutLoginResponse>(HttpStatusCode.OK, payload);
            }
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
