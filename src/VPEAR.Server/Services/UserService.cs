// <copyright file="UserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

namespace VPEAR.Server.Services
{
    /// <summary>
    /// Implements the <see cref="IUserService"/> interface.
    /// </summary>
    public class UserService : IUserService
    {
        /// <inheritdoc/>
        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> GetAsync(Guid? id = null, string? rule = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PostRegisterAsync(PostRegisterRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutAsync(Guid id, PutUserRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutLoginAsync(Guid id, PutLoginRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Response> PutLogoutasync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
