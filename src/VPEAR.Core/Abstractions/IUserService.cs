// <copyright file="IUserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    public interface IUserService
    {
        Task<Response> GetAsync(Guid? id = null, string? role = null);

        Task<Response> PutAsync(Guid id, PutUserRequest request);

        Task<Response> DeleteAsync(Guid id);

        Task<Response> PostRegisterAsync(PostRegisterRequest request);

        Task<Response> PutLoginAsync(Guid id, PutLoginRequest request);

        Task<Response> PutLogoutasync(Guid id);
    }
}
