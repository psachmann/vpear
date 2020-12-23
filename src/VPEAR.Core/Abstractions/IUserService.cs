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
        Task<Result> GetAsync(string? id = null, string? role = null);

        Task<Result> PutAsync(string id, PutUserRequest request);

        Task<Result> DeleteAsync(string id);

        Task<Result> PostRegisterAsync(PostRegisterRequest request);

        Task<Result> PutLoginAsync(PutLoginRequest request);
    }
}
