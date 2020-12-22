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
        Task<Response> GetAsync(string? id = null, string? role = null);

        Task<Response> PutAsync(string id, PutUserRequest request);

        Task<Response> DeleteAsync(string id);

        Task<Response> PostRegisterAsync(PostRegisterRequest request);

        Task<Response> PutLoginAsync(PutLoginRequest request);
    }
}
